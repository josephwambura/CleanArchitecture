using System.ComponentModel.DataAnnotations;

using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;

public class EmailAlertViewModel
{
  public string? From { get; set; }
  public string? EmailMessageTo { get; set; }
  public string? EmailMessageCC { get; set; }
  public string? EmailMessageSubject { get; set; }
  public string? EmailMessageBody { get; set; }
  public string? EmailMessageAttachments { get; set; }
  public bool EmailMessageIsBodyHtml { get; set; }
  public bool EmailMessageSecurityCritical { get; set; }
  public byte DLRStatus { get; set; }
  public string? Reference { get; set; }
  public byte Origin { get; set; }
  public string? Catalyst { get; set; }
  public byte Priority { get; set; }
  public int SendRetry { get; set; }

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
