using System.ComponentModel.DataAnnotations;

using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.DTO.MessagingModule.TextAlertAggregate;

public class TextAlertViewModel
{
  public string? TextMessageRecipient { get; set; }
  public string? TextMessageBody { get; set; }
  public bool TextMessageSecurityCritical { get; set; }
  public byte DlrStatus { get; set; }
  public byte Status { get; set; }
  public string? StatusDescription { get; set; }
  public string? Reference { get; set; }
  public byte Origin { get; set; }
  public byte Priority { get; set; }
  public byte SendRetry { get; set; }

  #region Common

  public Guid Id { get; set; }
  [Display(Name = "Created Date")]
  public DateTime CreatedDate { get; set; }
  [Display(Name = "Created Date")]
  public string? FormattedCreatedDate => CreatedDate.ISO8601DateTimeFormat();
  [Display(Name = "Created By")]
  public string? CreatedBy { get; set; }
  [Display(Name = "Modified Date")]
  public DateTime? ModifiedDate { get; set; }
  [Display(Name = "Modified Date")]
  public string? FormattedModifiedDate => ModifiedDate?.ISO8601DateTimeFormat();
  [Display(Name = "Modified By")]
  public string? ModifiedBy { get; set; }
  public byte RecordStatus { get; set; }
  public string? RecordStatusDescription => ((RecordStatus)RecordStatus).GetDescription();

  #endregion
}
