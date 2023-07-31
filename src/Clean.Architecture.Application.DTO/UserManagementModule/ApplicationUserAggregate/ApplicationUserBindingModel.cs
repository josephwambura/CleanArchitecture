using System.ComponentModel.DataAnnotations;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;

public class ApplicationUserBindingModel
{
  public string? Id { get; set; }
  [Required]
  public string? UserName { get; set; }

  /// <summary>
  ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
  ///     directly from your code. This API may change or be removed in future releases.
  /// </summary>
  [Required]
  [EmailAddress]
  [Display(Name = "Email")]
  public string? Email { get; set; }

  public bool EmailConfirmed { get; set; }

  [Phone]
  public string? PhoneNumber { get; set; }
  public bool PhoneNumberConfirmed { get; set; }

  [MaxLength(100)]
  public string? FirstName { get; set; } = default!;

  [MaxLength(100)]
  public string? MiddleName { get; set; } = default!;

  [MaxLength(100)]
  public string? LastName { get; set; } = default!;

  [MaxLength(256)]
  public string? ProfilePicture { get; set; } = default!;
  
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
