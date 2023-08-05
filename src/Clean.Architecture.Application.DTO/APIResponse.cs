namespace Clean.Architecture.Application.DTO;

public class APIResponse
{
  public int? Status { get; set; }
  public string? Message { get; set; }
  public dynamic? Data { get; set; }
}
