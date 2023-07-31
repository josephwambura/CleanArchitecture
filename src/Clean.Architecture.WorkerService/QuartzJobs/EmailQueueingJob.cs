using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Models;

using MassTransit;

using Quartz;

namespace Clean.Architecture.WorkerService.QuartzJobs;

public class EmailQueueingJob : IJob, IDisposable
{
  private readonly ILogger<EmailQueueingJob> _logger;
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly IBusControl _bus;

  public EmailQueueingJob(ILogger<EmailQueueingJob> logger, IHttpClientFactory httpClientFactory, IBusControl bus)
  {
    _logger = logger;
    _httpClientFactory = httpClientFactory;
    _bus = bus;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    _logger.LogInformation("{Job} job executing, triggered by {Trigger}", context.JobDetail.Key, context.Trigger.Key);

    // Create a new HttpClient
    var httpClient = _httpClientFactory.CreateClient("APIClient");

    //var emails = await httpClient.GetAndDeSerializeAsync<IEnumerable<EmailAlertDTO>>("api/EmailAlerts", _logger);
    var emails = await httpClient.GetAndDeSerializeAsync<IEnumerable<EmailAlertDTO>>("api/EmailAlerts/Pending?pageSize=100", _logger); // string searchText, int pageNumber = 0, int pageSize

    emails?.ToList()?.ForEach(async email =>
    {
      _logger.LogInformation($"Email: {email.Id} {email.EmailMessageSubject} {email.CreatedDate}");
      await _bus.Publish(new QueueDTO { RecordId = email.Id.ToString(), Remarks = $"The time is {DateTimeOffset.Now}" });
    });
  }

  public void Dispose()
  {
    _logger.LogInformation("Email Queueing job disposing");
    GC.SuppressFinalize(this);
  }
}
