using Microsoft.AspNetCore.Authorization;

namespace Clean.Architecture.Web.Pages;

[Authorize]
public abstract class AuthorizedPageModelBase : BasePageModelBase
{

}
