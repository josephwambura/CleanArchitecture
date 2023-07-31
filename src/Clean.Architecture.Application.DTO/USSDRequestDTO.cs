namespace Clean.Architecture.Application.DTO;

public class USSDRequestDTO
{
  public string serviceCode { get; set; } = default!;

  public string sessionId { get; set; } = default!;

  public string phoneNumber { get; set; } = default!;

  public string text { get; set; } = default!;
}
