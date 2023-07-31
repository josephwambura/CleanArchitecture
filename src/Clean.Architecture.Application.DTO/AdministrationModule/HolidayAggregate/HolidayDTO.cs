using System.ComponentModel.DataAnnotations;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Application.DTO.AdministrationModule.HolidayAggregate;

public class HolidayDTO
{
  [Display(Name = "Id")]
  public Guid Id { get; set; }

  [Display(Name = "Day")]
  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
  public DateTime Day { get; set; }

  [Display(Name = "Day To String")]
  public string DayToString => Day.ToString("d");

  [Display(Name = "Description")]
  public string? Description { get; set; }

  public DateTime CreatedDate { get; set; }
  public string? FormattedCreatedDate => CreatedDate.ISO8601DateTimeFormat();
  [Display(Name = "Created By")]
  public string? CreatedBy { get; set; }
  public DateTime? ModifiedDate { get; set; }
  public string? FormattedModifiedDate => ModifiedDate?.ISO8601DateTimeFormat();
  public string? ModifiedBy { get; set; }

  [Display(Name = "Is Recurring")]
  public bool IsRecurring { get; set; }
}
