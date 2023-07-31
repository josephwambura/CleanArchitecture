namespace Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;

public record ApplicationUserRecord(Guid Id, string UserName, string Email, string PhoneNumber);
