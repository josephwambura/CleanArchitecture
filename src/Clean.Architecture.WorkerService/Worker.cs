using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Models;

using Cronos;

using MassTransit;

namespace Clean.Architecture.WorkerService;

public class Worker : BackgroundService
{
  private const string schedule = "*/1 * * * *"; // run every 1 minutes
  //private const string schedule = "0 0 1 * *"; // run on the first day of every month
  private readonly CronExpression _cron;

  private readonly ILogger<Worker> _logger;
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly IBusControl _bus;

  public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory, IBusControl bus)
  {
    _logger = logger;
    _cron = CronExpression.Parse(schedule);
    _httpClientFactory = httpClientFactory;
    _bus = bus;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

      try
      {
        // Create a new HttpClient
        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var emails = await httpClient.GetAndDeSerializeAsync<IEnumerable<EmailAlertDTO>>("api/EmailAlerts/Pending?pageSize=100", _logger); // string searchText, int pageNumber = 0, int pageSize

        emails?.ToList()?.ForEach(async email =>
        {
          _logger.LogInformation($"Email: {email.Id} {email.EmailMessageSubject} {email.CreatedDate}");
          await _bus.Publish(new QueueDTO { RecordId = email.Id.ToString(), Remarks = $"The time is {DateTimeOffset.Now}" });
        });
      }
      catch (Exception es)
      {
        _logger.LogError("Worker running at: {message}", es.ToString());
        throw;
      }
      finally
      {
        var utcNow = DateTime.UtcNow;
        var nextUtc = _cron.GetNextOccurrence(utcNow);
        await Task.Delay(nextUtc!.Value - utcNow, stoppingToken);
      }


    }
  }
}
