using Ardalis.ListStartupServices;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Clean.Architecture.Application;
using Clean.Architecture.Domain;
using Clean.Architecture.Infrastructure;
using Clean.Architecture.Infrastructure.Data.Auth;
using Clean.Architecture.Web;

using FastEndpoints;
using FastEndpoints.ApiExplorer;
using FastEndpoints.Swagger.Swashbuckle;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

using ReturnTrue.AspNetCore.Identity.Anonymous;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddW3CLogging(logging =>
{
  // Log all W3C fields
  logging.LoggingFields = W3CLoggingFields.All;

  //logging.FileSizeLimit = 5 * 1024 * 1024;
  logging.RetainedFileCountLimit = 2;
  //logging.FileName = "Clean.Architecture.Web" + DateTime.UtcNow.ToString("yyyyMMdd");
  logging.LogDirectory = @"D:\\Logs\\Clean.Architecture\\web_w3clogs";
  logging.FlushInterval = TimeSpan.FromSeconds(2);
});

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

string? mainConnectionString = builder.Configuration.GetConnectionString("SqliteConnection");  //Configuration.GetConnectionString("DefaultConnection");
string? authConnectionString = builder.Configuration.GetConnectionString("AuthConnection");  //Configuration.GetConnectionString("AuthConnection");

string? dockerMainConnectionString = builder.Configuration.GetConnectionString("MainConnection_Docker");  //Configuration.GetConnectionString("MainConnection_Docker");
string? dockerAuthConnectionString = builder.Configuration.GetConnectionString("AuthConnection_Docker");  //Configuration.GetConnectionString("AuthConnection_Docker");

string? defaultMainConnection = builder.Configuration.GetConnectionString("DefaultMainConnection");  //Configuration.GetConnectionString("DefaultMainConnection");
string? defaultAuthConnection = builder.Configuration.GetConnectionString("DefaultAuthConnection");  //Configuration.GetConnectionString("DefaultAuthConnection");

var appSettingsConfiguration = builder.Configuration.GetSection(nameof(AppSettingsConfiguration)).Get<AppSettingsConfiguration>() ?? throw new InvalidOperationException("Configuration section 'RedisConfiguration' not found.");
builder.Services.AddSingleton(appSettingsConfiguration);

switch ((RunningEnvironment)appSettingsConfiguration.RunningEnvironment)
{
  case RunningEnvironment.WindowsServerIIS:
  case RunningEnvironment.VisualStudioDebugSQLServer:
    builder.Services.AddDbContext(defaultMainConnection!, appSettingsConfiguration.RunningEnvironment);
    builder.Services.AddAuthDbContext(defaultAuthConnection!, appSettingsConfiguration.RunningEnvironment);
    break;
  case RunningEnvironment.DockerContainer:
    builder.Services.AddDbContext(dockerMainConnectionString!, appSettingsConfiguration.RunningEnvironment);
    builder.Services.AddAuthDbContext(dockerAuthConnectionString!, appSettingsConfiguration.RunningEnvironment);
    break;
  case RunningEnvironment.VisualStudioDebug:
  default:
    builder.Services.AddDbContext(mainConnectionString!, appSettingsConfiguration.RunningEnvironment);
    builder.Services.AddAuthDbContext(authConnectionString!, appSettingsConfiguration.RunningEnvironment);
    break;
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// needed to load configuration from appsettings.json
builder.Services.AddOptions();

builder.Services.Configure<RouteOptions>(options =>
{
  options.LowercaseUrls = true;
  options.LowercaseQueryStrings = true;
});

builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(2);
  //options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});

var redisConfiguration = builder.Configuration.GetSection(nameof(RedisConfiguration)).Get<RedisConfiguration>() ?? throw new InvalidOperationException("Configuration section 'RedisConfiguration' not found.");
builder.Services.AddSingleton(redisConfiguration);

builder.Services.AddStackExchangeRedisCache(setupAction =>
{
  setupAction.Configuration = $"{redisConfiguration?.Host}:{redisConfiguration?.Port},password={redisConfiguration?.Password},abortConnect={redisConfiguration?.AbortConnect}";
});

builder.Services.AddLazyCache();

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();
builder.Services.AddFastEndpoints();
builder.Services.AddFastEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Clean.Architecture API",
    Version = "v1",
    Description = "Clean.Architecture API V1",
    Contact = new OpenApiContact
    {
      Name = "Clean.Architecture API",
      Email = "engineerwambura@gmail.com",
      Url = new Uri("https://github.com/josephwambura/CleanArchitecture")
    }
  });
  c.EnableAnnotations();
  c.OperationFilter<FastEndpointsOperationFilter>();
});

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // optional - default path to view services is /listallservices - recommended to choose your own path
  config.Path = "/listservices";
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultApplicationModule());
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

//builder.Logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure

// Configure a fixed window policy with a limit of 10 requests per minute
builder.Services.AddRateLimiter(_ =>
    _.AddFixedWindowLimiter("fixed", options =>
    {
      options.PermitLimit = 10;
      options.Window = TimeSpan.FromMinutes(1);
    }));

builder.Services.AddLocalization(options =>
{
  options.ResourcesPath = "Resources";
});

var app = builder.Build();

app.UseW3CLogging();

app.UseAnonymousId(new AnonymousIdCookieOptionsBuilder()
    .SetCustomCookieName("Clean.Architecture_Anonymous_CT")
    .SetCustomCookieRequireSsl(true) //Uncomment this in the case of usign SSL, such as the default setup of .NET Core 2.1 
    .SetCustomCookieTimeout(120)
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseMigrationsEndPoint();
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
app.UseRouting();
app.UseFastEndpoints();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseAuthorization();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean.Architecture API V1"));

// Use the rate limiter middleware
app.UseRateLimiter();

//app.MapGet("/", () => "Hello World!")
//    // Apply the fixed window policy to this endpoint
//    .RequireRateLimiting("fixed");

app.UseSession();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
  Predicate = check => check.Tags.Contains("ready"),
  //ResponseWriter = HealthCheckResponseWriter,
  ResultStatusCodes =
  {
      [HealthStatus.Healthy] = StatusCodes.Status200OK,
      [HealthStatus.Degraded] = StatusCodes.Status200OK,
      [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
  }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
  //ResponseWriter = HealthCheckResponseWriter,
  ResultStatusCodes =
  {
      [HealthStatus.Healthy] = StatusCodes.Status200OK,
      [HealthStatus.Degraded] = StatusCodes.Status200OK,
      [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
  }
}); //.RequireAuthorization();

app.UseSerilogRequestLogging(options =>
{
  options.MessageTemplate = "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

  //// Emit debug-level events instead of the defaults
  //options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

  // Attach additional properties to the request completion event
  options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
  {
    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
  };
});

// Seed Database
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<ApplicationDbContext>();
    //                    context.Database.Migrate();
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
    SeedData.InitializeAuth(services);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the auth DB. {exceptionMessage}", ex.Message);
  }

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    //                    context.Database.Migrate();
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
    SeedData.Initialize(services);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the main DB. {exceptionMessage}", ex.Message);
  }
}

await app.RunAsync();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
  //static Task HealthCheckResponseWriter(HttpContext context, HealthReport result)
  //{
  //  context.Response.ContentType = "application/json";

  //  return context.Response.WriteAsync(result.ToJsonString());
  //}
}
