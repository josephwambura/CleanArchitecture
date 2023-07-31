using System.ComponentModel.DataAnnotations;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Application.DTO.AdministrationModule.EnumerationAggregate;

public class EnumerationDTO
{
  [Display(Name = "Key")]
  public string? Key { get; set; }

  [Display(Name = "Value")]
  public int Value { get; set; }

  [Display(Name = "Description")]
  public string? Description { get; set; }

  #region Common

  public Guid Id { get; set; }
  [Display(Name = "Created Date")]
  public DateTimeOffset CreatedDate { get; set; }
  public string? FormattedCreatedDate => CreatedDate.ISO8601DateTimeFormat();
  [Display(Name = "Created By")]
  public string? CreatedBy { get; set; }
  public DateTimeOffset? ModifiedDate { get; set; }
  public string? FormattedModifiedDate => ModifiedDate?.ISO8601DateTimeFormat();
  public string? ModifiedBy { get; set; }
  public byte RecordStatus { get; set; }

  #endregion
}
