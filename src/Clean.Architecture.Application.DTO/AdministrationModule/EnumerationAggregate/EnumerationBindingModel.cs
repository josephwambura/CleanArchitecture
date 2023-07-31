using System.ComponentModel.DataAnnotations;

namespace Clean.Architecture.Application.DTO.AdministrationModule.EnumerationAggregate;

public class EnumerationBindingModel
{
  public Guid? Id { get; set; }
  [Display(Name = "Key")]
  [Required]
  public string? Key { get; set; }

  [Display(Name = "Value")]
  [Required]
  public int Value { get; set; }

  [Display(Name = "Description")]
  [Required]
  public string? Description { get; set; }
}
