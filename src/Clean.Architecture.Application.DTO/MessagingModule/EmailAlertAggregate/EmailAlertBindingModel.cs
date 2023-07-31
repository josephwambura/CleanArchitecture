using System.ComponentModel.DataAnnotations;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;

public class EmailAlertBindingModel
{
  public Guid? Id { get; set; }
  [Required]
  [EmailAddress]
  public string? From { get; set; }
  [Required]
  [EmailAddress]
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
