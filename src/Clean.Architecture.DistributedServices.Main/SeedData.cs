using System.Diagnostics;

using Clean.Architecture.Core;
using Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.Core.ValueObjects;
using Clean.Architecture.Core.WeatherForecastAggregate;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Data.Auth;
using Clean.Architecture.SharedKernel.Utils;

using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.DistributedServices.Main;

public static class SeedData
{
  public static readonly SystemInfo SystemInfo1 = new SystemInfo();
  public static readonly ServiceHeader ServiceHeader1 = new ServiceHeader
  {
    EnvironmentIPAddress = SystemInfo1.IPAddress,
    EnvironmentMACAddress = SystemInfo1.MACAddress,
    EnvironmentMotherboardSerialNumber = SystemInfo1.MotherboardSerialNumber,
    EnvironmentProcessorId = SystemInfo1.ProcessorId,
    EnvironmentUserName = SystemInfo1.UserName,
    EnvironmentMachineName = SystemInfo1.MachineName,
    EnvironmentDomainName = SystemInfo1.DomainName,
    EnvironmentOSVersion = Environment.OSVersion.ToString(),
    ApplicationUserName = "_SYS_"
  };
  public static readonly TransactionEnvironment TransactionEnvironment1 = new TransactionEnvironment
  {
    EnvironmentIPAddress = ServiceHeader1.EnvironmentIPAddress,
    EnvironmentMACAddress = ServiceHeader1.EnvironmentMACAddress,
    EnvironmentMotherboardSerialNumber = ServiceHeader1.EnvironmentMotherboardSerialNumber,
    EnvironmentProcessorId = ServiceHeader1.EnvironmentProcessorId,
    EnvironmentUserName = ServiceHeader1.EnvironmentUserName,
    EnvironmentMachineName = ServiceHeader1.EnvironmentMachineName,
    EnvironmentDomainName = ServiceHeader1.EnvironmentDomainName,
    EnvironmentOSVersion = ServiceHeader1.EnvironmentOSVersion,

    ThirdPartyClientIPAddress = ServiceHeader1.ClientIPAddress,

    ClientIPAddress = ServiceHeader1.ClientIPAddress,
    ClientUserAgent = ServiceHeader1.ClientUserAgent,
    ClientBrowser = ServiceHeader1.ClientBrowser?.ToString(),
    ClientLocation = ServiceHeader1.ClientLocation?.ToString(),
  };
  public static readonly Contributor Contributor1 = new("Joseph")
  {
    Id = Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1")
  };
  public static readonly Contributor Contributor2 = new("Kelsey");
  public static readonly Project TestProject1 = new Project("Test Project", PriorityStatus.Backlog)
  {
    Id = Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1")
  };
  public static readonly ToDoItem ToDoItem1 = new ToDoItem
  {
    Title = "Get Sample Working",
    Description = "Try to get the sample to build.",
    Id = Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1")
  };
  public static readonly ToDoItem ToDoItem2 = new ToDoItem
  {
    Title = "Review Solution",
    Description = "Review the different projects in the solution and how they relate to one another."
  };
  public static readonly ToDoItem ToDoItem3 = new ToDoItem
  {
    Title = "Run and Review Tests",
    Description = "Make sure all the tests run and review what they are doing."
  };
  public static readonly StaticSetting StaticSetting1 = StaticSettingFactory.CreateStaticSetting(DefaultSettings.Instance.EmailMessageFrom!, "noreply@cleanarchitecture.com", true, false, (byte)RecordStatus.Approved, ServiceHeader1);
  public static readonly string[] summaries = new[]
  {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using (var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
    {
      // Look for any TODO items.
      if (dbContext.ToDoItems.Any())
      {
        return;   // DB has been seeded
      }

      var logger = serviceProvider.GetRequiredService<ILogger<AppDbContext>>();

      PopulateTestData(dbContext, logger);

      #region View
      //FormattableString query = $"SELECT * FROM {Utility.DbTableName<Project>()}";
      dbContext.Database.ExecuteSqlRaw(Dashboard.View);
      #endregion
    }
  }

  public static void InitializeAuth(IServiceProvider serviceProvider)
  {
    using (var dbContext = new ApplicationDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>(), null))
    {
      // Look for any Users.
      if (dbContext.Users.Any())
      {
        return;   // DB has been seeded
      }

      PopulateAuthTestData(dbContext);
    }
  }
  public static void PopulateTestData(AppDbContext dbContext, ILogger<AppDbContext> logger)
  {
    logger.LogInformation("Seeding data...");

    foreach (var item in dbContext.Projects)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.ToDoItems)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.Contributors)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.StaticSettings)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.WeatherForecasts)
    {
      dbContext.Remove(item);
    }
    dbContext.SaveChanges();

    dbContext.StaticSettings.Add(StaticSetting1);

    dbContext.SaveChanges();

    dbContext.WeatherForecasts.AddRangeAsync(Enumerable.Range(1, 10).Select(index => new WeatherForecast
    {
      Id = Guid.NewGuid(),
      Date = DateTime.Now.AddDays(index),
      CreatedDate = DateTimeOffset.Now.AddDays(-(Random.Shared.Next(index))),
      CreatedBy = "__SYS__",
      TemperatureC = Random.Shared.Next(-20, 55),
      Summary = summaries[Random.Shared.Next(summaries.Length)]
    })
            .ToArray());

    dbContext.SaveChanges();

    Contributor1.CreatedBy = "_SYS_";
    Contributor2.CreatedBy = "_SYS_";

    dbContext.Contributors.Add(Contributor1);
    dbContext.Contributors.Add(Contributor2);

    dbContext.SaveChanges();

    ToDoItem1.CreatedBy = "_SYS_";
    ToDoItem2.CreatedBy = "_SYS_";
    ToDoItem3.CreatedBy = "_SYS_";

    ToDoItem1.AddContributor(Contributor1.Id);
    ToDoItem2.AddContributor(Contributor2.Id);
    ToDoItem3.AddContributor(Contributor1.Id);

    TestProject1.CreatedBy = "_SYS_";

    TestProject1.AddItem(ToDoItem1);
    TestProject1.AddItem(ToDoItem2);
    TestProject1.AddItem(ToDoItem3);
    dbContext.Projects.Add(TestProject1);

    var projects = new List<Project>();

    logger.LogInformation("Generating projects.");

    int batchSize = 100000; // Adjust this as needed

    for (int i = 0; i < (dbContext.Database.IsSqlServer() ? batchSize : dbContext.Database.IsSqlite() ? 500 : 0); i++)
    {
      var testProject = ProjectFactory.CreateProject($"Test Project {i}", PriorityStatus.Backlog, (byte)RecordStatus.Approved, ServiceHeader1);

      testProject.AddItem(ToDoItemFactory.CreateToDoItem(
        $"Get Sample Working {i}",
        "Try to get the sample to build.",
        null,
        (byte)RecordStatus.Approved,
        ServiceHeader1
      ));
      testProject.AddItem(ToDoItemFactory.CreateToDoItem(
        $"Review Solution {i}",
        "Review the different projects in the solution and how they relate to one another.",
        null,
        (byte)RecordStatus.Approved,
        ServiceHeader1
      ));
      testProject.AddItem(ToDoItemFactory.CreateToDoItem(
        $"Run and Review Tests {i}",
        "Make sure all the tests run and review what they are doing.",
        null,
        (byte)RecordStatus.Approved,
        ServiceHeader1
      ));

      projects.Add(testProject);
    }

    if (dbContext.Database.IsSqlServer())
    {
      logger.LogInformation($"Seeding {projects.Count} projects.");
      dbContext.BulkInsert(projects, Utility.DbTableName<Project>(), ServiceHeader1);

      var objects = projects.SelectMany(project => project.Items.Select(i => new { i.Id, i.CreatedDate, i.ModifiedDate, i.IsDone, i.CreatedBy, i.ContributorId, ProjectId = project.Id, i.Description, i.Title, i.RecordStatus, })).ToList();

      // Create and start a Stopwatch instance
      var stopwatch = Stopwatch.StartNew();

      #region Loop, not used though
      //int totalBatches = objects.Count / batchSize;
      //for (int i = 0; i < totalBatches; i++)
      //{
      //  var batch = objects.Skip(i * batchSize).Take(batchSize).ToList();
      //  dbContext.BulkInsert(batch, Utility.DbTableName<ToDoItem>(), ServiceHeader1);
      //}
      //// Handle the remaining objects if any
      //int remainder = objects.Count % batchSize;
      //if (remainder > 0)
      //{
      //  var batch = objects.Skip(totalBatches * batchSize).Take(remainder).ToList();
      //  dbContext.BulkInsert(batch, Utility.DbTableName<ToDoItem>(), ServiceHeader1);
      //}

      //// Stop the stopwatch
      //stopwatch.Stop();
      #endregion

      var batches = objects.Select((o, i) => new { o, i })
                     .GroupBy(x => x.i / batchSize)
                     .Select(g => g.Select(x => x.o).ToList());

      logger.LogInformation($"Seeding {batches.Count()} todo items batches.");

      foreach (var batch in batches)
      {
        dbContext.BulkInsert(batch, Utility.DbTableName<ToDoItem>(), ServiceHeader1);
      }

      // Stop the stopwatch
      stopwatch.Stop();

      var elapsedMs = stopwatch.ElapsedMilliseconds;

      logger.LogInformation($"Seeding todoItems took: {elapsedMs} Milliseconds.");
    }
    else if (dbContext.Database.IsSqlite())
    {
      logger.LogInformation($"Seeding {projects.Count} projects.");
      dbContext.Projects.AddRange(projects);
    }
    else
    {
      // Some tests expect 1 project to be successful.
    }

    dbContext.SaveChanges();

    logger.LogInformation("Seeding completed.");
  }

  public static void PopulateAuthTestData(ApplicationDbContext dbContext)
  {
    foreach (var item in dbContext.Users)
    {
      dbContext.Remove(item);
    }
    dbContext.SaveChanges();

    var applicationUsers = new List<ApplicationUser>();

    for (int i = 0; i < 7; i++)
    {
      var testApplicationUser = ApplicationUserFactory.CreateApplicationUser($"User_{i}", $"email_{i}@gmail.com", true, $"072{i}87{i}52{i}", true, (byte)RecordStatus.Approved, ServiceHeader1);

      applicationUsers.Add(testApplicationUser);
    }

    applicationUsers.ForEach(applicationUser => dbContext.Users.Add(applicationUser));

    dbContext.SaveChanges();
  }
}
