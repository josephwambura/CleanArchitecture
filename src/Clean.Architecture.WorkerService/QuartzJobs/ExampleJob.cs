﻿using Quartz;

namespace Clean.Architecture.WorkerService.QuartzJobs;

public class ExampleJob : IJob, IDisposable
{
  private readonly ILogger<ExampleJob> logger;

  public ExampleJob(ILogger<ExampleJob> logger)
  {
    this.logger = logger;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    logger.LogInformation("{Job} job executing, triggered by {Trigger}", context.JobDetail.Key, context.Trigger.Key);
    await Task.Delay(TimeSpan.FromSeconds(1));
  }

  public void Dispose()
  {
    logger.LogInformation("Example job disposing");
    GC.SuppressFinalize(this);
  }
}