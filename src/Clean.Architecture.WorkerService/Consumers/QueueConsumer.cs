using System.Text;
using System.Text.Json;

using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Models;

using MassTransit;

namespace Clean.Architecture.WorkerService.Consumers;

public class QueueConsumer : IConsumer<QueueDTO>
{
  private readonly ILogger<QueueConsumer> _logger;
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly IBusControl _bus;

  public QueueConsumer(ILogger<QueueConsumer> logger, IHttpClientFactory httpClientFactory, IBusControl bus)
  {
    _logger = logger;
    _httpClientFactory = httpClientFactory;
    _bus = bus;
  }

  public async Task Consume(ConsumeContext<QueueDTO> context)
  {
    // Create a new HttpClient
    var httpClient = _httpClientFactory.CreateClient("APIClient");

    var emailAlertDTO = await httpClient.GetAndDeSerializeAsync<EmailAlertDTO>($"api/EmailAlerts/{context.Message.RecordId}", _logger);

    var jsonContent = new StringContent(JsonSerializer.Serialize<object?>(null), Encoding.UTF8, "application/json");

    //var response = await httpClient.PatchAndDeSerializeAsync($"api/projects/{emailAlertDTO}/complete", jsonContent, _logger);



    _logger.LogInformation("{RecordId}: Received Text: {Text}", context.Message.RecordId, context.Message.Remarks);
    _logger.LogInformation("{Email}: ", emailAlertDTO.ToJson());
    //return Task.CompletedTask;
  }
}
