namespace Clean.Architecture.SharedKernel.Models;

public record QueueDTO()
{
  public string RecordId { get; init; } = default!;
  public string Remarks { get; init; } = default!;
}
