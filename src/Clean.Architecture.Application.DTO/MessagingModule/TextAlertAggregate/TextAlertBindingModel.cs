using System.ComponentModel.DataAnnotations;

namespace Clean.Architecture.Application.DTO.MessagingModule.TextAlertAggregate;

public class TextAlertBindingModel
{
  public Guid? Id { get; set; }
  [Required]
  [Phone]
  public string? TextMessageRecipient { get; set; }
  [Required]
  public string? TextMessageBody { get; set; }
  public bool TextMessageSecurityCritical { get; set; }
  public byte DlrStatus { get; set; }
  public byte Status { get; set; }
  public string? StatusDescription { get; set; }
  public string? Reference { get; set; }
  public byte Origin { get; set; }
  public byte Priority { get; set; }
  public byte SendRetry { get; set; }
}
