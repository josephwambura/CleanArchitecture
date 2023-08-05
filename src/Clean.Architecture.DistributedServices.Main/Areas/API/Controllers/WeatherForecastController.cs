using Clean.Architecture.Application.DTO.WeatherForecastAggregate;
using Clean.Architecture.DistributedServices.Main.Api;

namespace Clean.Architecture.DistributedServices.Main.Areas.API.Controllers;

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
  public async Task<List<WeatherForecastDTO>> GetAsync(CancellationToken cancellationToken)
    => (await _channelService.FindWeatherForecastsAsync(this.GetServiceHeader(User), cancellationToken)).Value;

  [HttpGet("GetWeatherForecastWithFiltersAndInPage")]
  public async Task<IActionResult> GetWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken)
    => Ok((await _channelService.GetWeatherForecastsWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, this.GetServiceHeader(User), cancellationToken)).Value);
}
