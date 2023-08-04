using Clean.Architecture.Application.DTO.WeatherForecastAggregate;

namespace Clean.Architecture.DistributedServices.Main.Api;

public class WeatherForecastController : BaseApiController
{
  private readonly ILogger<WeatherForecastController> _logger;
  private readonly IChannelService _channelService;

  public WeatherForecastController(ILogger<WeatherForecastController> logger, IChannelService channelService)
  {
    _logger = logger;
    _channelService = channelService;
  }

  [HttpGet(Name = "GetWeatherForecast")]
  public async Task<List<WeatherForecastDTO>> GetAsync()
  {
    return (await _channelService.FindWeatherForecastsAsync(this.GetServiceHeader(User))).Value;
  }
}
