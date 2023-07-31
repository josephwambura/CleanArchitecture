using System.ComponentModel.DataAnnotations;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Application.DTO.InventoryModule.ProductAggregate;

public class ProductBindingModel
{
  public Guid Id { get; set; }
  [Required]
  [MaxLength(120)]
  public string Name { get; set; } = string.Empty;
  public decimal Price { get; set; }

  #region Common Properties

  public DateTime CreatedDate { get; set; }
  public string? FormattedCreatedDate => CreatedDate.ISO8601DateTimeFormat();
  public DateTime? ModifiedDate { get; set; }
  public string? FormattedModifiedDate => ModifiedDate?.ISO8601DateTimeFormat();
  public string CreatedBy { get; set; } = default!;
  public string? ModifiedBy { get; set; }
  public byte RecordStatus { get; set; }

  #endregion
}
