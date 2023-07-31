using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;

public class ApplicationUserDTO
{
  public string? Password { get; set; }
  public string? ConfirmPassword { get; set; }

  public string Id { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public string NormalizedUserName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string NormalizedEmail { get; set; } = string.Empty;
  public bool EmailConfirmed { get; set; }
  public string PhoneNumber { get; set; } = string.Empty;
  public bool PhoneNumberConfirmed { get; set; }
  public string? FirstName { get; set; } = default!;
  public string? MiddleName { get; set; } = default!;
  public string? LastName { get; set; } = default!;
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
