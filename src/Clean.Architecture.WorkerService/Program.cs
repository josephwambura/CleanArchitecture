using Clean.Architecture.SharedKernel.Configurations;
using Clean.Architecture.WorkerService.Consumers;
using Clean.Architecture.WorkerService.QuartzJobs;

using MassTransit;

using Quartz;

using Serilog;

Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

var host = CreateHostBuilder(args).Build();

//var pluginProvider = host.Services.GetService<PluginProvider>();
//pluginProvider?.Initialize();

await host.RunAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)

      //.UseServiceProviderFactory(new AutofacServiceProviderFactory())

      .UseSerilog()

      //.ConfigureContainer<ContainerBuilder>(containerBuilder =>
      //{
      //  containerBuilder.RegisterModule(new DefaultApplicationModule());
      //  containerBuilder.RegisterModule(new DefaultCoreModule());
      //  containerBuilder.RegisterModule(new DefaultInfrastructureModule(true));
      //})

      .ConfigureServices((hostContext, services) =>
      {
        var appSettingsConfiguration = hostContext.Configuration.GetSection("AppSettingsConfiguration").Get<AppSettingsConfiguration>() ?? throw new InvalidOperationException("Configuration section 'AppSettingsConfiguration' not found.");

        var redisConfiguration = hostContext.Configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>() ?? throw new InvalidOperationException("Configuration section 'RedisConfiguration' not found.");

        var rabbitMqConfiguration = hostContext.Configuration.GetSection("RabbitMqConfiguration").Get<RabbitMqConfiguration>() ?? throw new InvalidOperationException("Configuration section 'RabbitMqConfiguration' not found.");

        //services.AddSingleton<IEmailSender, SmtpEmailSender>();
        //services.AddSingleton<PluginProvider>();
        //services.AddHostedService<Worker>();

        services.AddHttpClient();

        services.AddHttpClient("APIClient", client =>
        {
          client.BaseAddress = new Uri(appSettingsConfiguration.WebApplicationAddress!);
        });

        // if you are using persistent job store, you might want to alter some options
        services.Configure<QuartzOptions>(options =>
        {
          options.Scheduling.IgnoreDuplicates = true; // default: false
          options.Scheduling.OverWriteExistingData = true; // default: true
        });

        // base configuration for DI
        services.AddQuartz(q =>
        {
          // handy when part of cluster or you want to otherwise identify multiple schedulers
          q.SchedulerId = "Scheduler-Core";

          var loggerFactory = new LoggerFactory()
              .AddSerilog(Log.Logger);

          //q.SetLoggerFactory(loggerFactory);

          // we take this from appsettings.json, just show it's possible
          // q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";

          // this is default configuration if you don't alter it
          q.UseMicrosoftDependencyInjectionJobFactory();

          // these are the defaults
          q.UseSimpleTypeLoader();
          q.UseInMemoryStore();
          q.UseDefaultThreadPool(tp =>
          {
            tp.MaxConcurrency = 10;
          });

          #region Example Job

          // quickest way to create a job with single trigger is to use ScheduleJob
          q.ScheduleJob<ExampleJob>(trigger => trigger
              .WithIdentity("Combined Configuration Trigger")
              .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
              .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
              .WithDescription("my awesome trigger configured for a job with single call")
          );

          // configure jobs with code
          var jobKey = new JobKey("awesome job", "awesome group");

          q.AddJob<ExampleJob>(j => j
              .StoreDurably()
              .WithIdentity(jobKey)
              .WithDescription("my awesome job")
          );

          q.AddTrigger(t => t
              .WithIdentity("Simple Trigger")
              .ForJob(jobKey)
              .StartNow()
              .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever())
              .WithDescription("my awesome simple trigger")
          );

          #endregion

          #region Email Queueing Job

          // quickest way to create a job with single trigger is to use ScheduleJob
          q.ScheduleJob<EmailQueueingJob>(trigger => trigger
              .WithIdentity("Email Queueing Trigger")
              .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
              .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
              .WithDescription("my email queueing trigger configured for a job with single call")
          );

          // configure jobs with code
          var jobKey2 = new JobKey("email queueing job", "email queueing group");

          q.AddJob<EmailQueueingJob>(j => j
              .StoreDurably()
              .WithIdentity(jobKey2)
              .WithDescription("my email queueing job")
          );

          q.AddTrigger(t => t
              .WithIdentity("Email Queueing Trigger")
              .ForJob(jobKey2)
              .StartNow()
              .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever())
              .WithDescription("my email queueing simple trigger")
          );

          #endregion

        });

        services.AddMassTransit(x =>
        {
          x.AddPublishMessageScheduler();

          x.AddQuartzConsumers();

          x.AddConsumer<QueueConsumer>();

          x.UsingRabbitMq((registrationContext, cfg) =>
          {
            cfg.Host(rabbitMqConfiguration.Host, rabbitMqConfiguration.VirtualHost, host =>
            {
              host.Username(rabbitMqConfiguration.Username);
              host.Password(rabbitMqConfiguration.Password);
            });

            cfg.UsePublishMessageScheduler();

            cfg.ConfigureEndpoints(registrationContext);
          });
        });

        services.Configure<MassTransitHostOptions>(options =>
        {
          options.WaitUntilStarted = true;
        });

        // Quartz.Extensions.Hosting hosting
        services.AddQuartzHostedService(options =>
        {
          // when shutting down we want jobs to complete gracefully
          options.WaitForJobsToComplete = true;

          // when we need to init another IHostedServices first
          options.StartDelay = TimeSpan.FromSeconds(10);
        });
      });


