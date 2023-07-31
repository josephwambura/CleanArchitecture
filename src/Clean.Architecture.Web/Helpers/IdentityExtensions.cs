using System.Security.Claims;
using System.Security.Principal;

namespace Clean.Architecture.Web.Helpers;

public static class IdentityExtensions
{
  public static string GetUserFirstName(this IIdentity identity)
  {
    var claim = ((ClaimsIdentity)identity).FindFirst(ApplicationUserProperties.FirstName);
    // Test for null to avoid issues during local testing
    return claim != null ? claim.Value : string.Empty;
  }
}

public static class ApplicationUserProperties
{
  public const string Id = "Id";

  public const string FirstName = "FirstName";

  public const string MiddleName = "MiddleName";

  public const string LastName = "LastName";

  public const string ProfilePicture = "ProfilePicture";

  public const string PhoneNumber = "PhoneNumber";

  public const string Email = "Email";
}
