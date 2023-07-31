using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Clean.Architecture.Application.DTO.AdministrationModule.HolidayAggregate;

public class HolidayBindingModel
{
  public Guid? Id { get; set; }
  [DataMember]
  [Display(Name = "Day")]
  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
  [Required]
  public DateTime Day { get; set; }

  [Required]
  [DataMember]
  [Display(Name = "Description")]
  public string? Description { get; set; }

  [Display(Name = "Is Recurring")]
  public bool IsRecurring { get; set; }
}
