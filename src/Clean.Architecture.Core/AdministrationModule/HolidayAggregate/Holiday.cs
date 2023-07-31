using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Core.AdministrationModule.HolidayAggregate;

public class Holiday : EntityBase, IAggregateRoot
{
  public DateTime Day { get; set; }

  public string Description { get; set; } = default!;

  public bool IsInternational { get; set; }
}
