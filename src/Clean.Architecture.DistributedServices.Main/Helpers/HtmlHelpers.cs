using System.Security.Claims;

using Clean.Architecture.Core.ValueObjects;

using Microsoft.Net.Http.Headers;

namespace Clean.Architecture.DistributedServices.Main;

public static class HtmlHelpers
{
  public static dynamic DataTablesJson<T>(this Controller controller, JQueryDataTablesFormRequest jQueryDataTablesFormRequest, PageCollectionInfo<T> pageCollectionInfo) where T : class
  {
    // Create a response object
    dynamic response = new JqueryResponse<T>
    {
      Draw = jQueryDataTablesFormRequest.Draw,
      RecordsTotal = pageCollectionInfo.ItemsCount,
      RecordsFiltered = !string.IsNullOrWhiteSpace(jQueryDataTablesFormRequest.Search?.Value) ? pageCollectionInfo.FilteredItemsCount : pageCollectionInfo.ItemsCount,
      Data = pageCollectionInfo.PageCollection ?? new List<T>(),
      Error = null
    };

    return response;
  }

  public static dynamic DataTablesJson<T>(int draw, PageCollectionInfo<T> pageCollectionInfo) where T : class
  {
    // Create a response object
    dynamic response = new JqueryResponse<T>
    {
      Draw = draw,
      RecordsTotal = pageCollectionInfo.ItemsCount,
      RecordsFiltered = pageCollectionInfo.FilteredItemsCount,
      Data = pageCollectionInfo.PageCollection ?? new List<T>(),
      Error = null
    };

    return response;
  }

  public static TransactionEnvironment GetTransactionEnvironment(this PageModel pageModel, ServiceHeader serviceHeader)
  {
    return new TransactionEnvironment
    {
      EnvironmentIPAddress = serviceHeader.EnvironmentIPAddress,
      EnvironmentMACAddress = serviceHeader.EnvironmentMACAddress,
      EnvironmentMotherboardSerialNumber = serviceHeader.EnvironmentMotherboardSerialNumber,
      EnvironmentProcessorId = serviceHeader.EnvironmentProcessorId,
      EnvironmentUserName = serviceHeader.EnvironmentUserName,
      EnvironmentMachineName = serviceHeader.EnvironmentMachineName,
      EnvironmentDomainName = serviceHeader.EnvironmentDomainName,
      EnvironmentOSVersion = serviceHeader.EnvironmentOSVersion,

      ThirdPartyClientIPAddress = serviceHeader.ClientIPAddress,

      ClientIPAddress = serviceHeader.ClientIPAddress,
      ClientUserAgent = serviceHeader.ClientUserAgent,
      ClientBrowser = serviceHeader.ClientBrowser?.ToString(),
      ClientLocation = serviceHeader.ClientLocation?.ToString(),
    };
  }

  public static ServiceHeader GetServiceHeader(this PageModel pageModel, ClaimsPrincipal claimsPrincipal)
  {
    var systemInfo = new SystemInfo();
    var clientRequestInfo = pageModel.RetrieveClientRequestInfo() ?? new ClientRequestInfo();

    return new ServiceHeader
    {
      EnvironmentIPAddress = systemInfo.IPAddress,
      EnvironmentMACAddress = systemInfo.MACAddress,
      EnvironmentMotherboardSerialNumber = systemInfo.MotherboardSerialNumber,
      EnvironmentProcessorId = systemInfo.ProcessorId,
      EnvironmentUserName = systemInfo.UserName,
      EnvironmentMachineName = systemInfo.MachineName,
      EnvironmentDomainName = systemInfo.DomainName,
      EnvironmentOSVersion = Environment.OSVersion.ToString(),
      ApplicationUserName = claimsPrincipal.Identity?.Name,

      ClientIPAddress = clientRequestInfo.UserHostAddress,
      ClientSessionID = clientRequestInfo.SessionID,
      ClientUserHostAddress = clientRequestInfo.UserHostAddress,
      ClientUserAgent = clientRequestInfo.UserAgent,
      ClientUserHostName = clientRequestInfo.UserHostName,
      ClientBrowser = clientRequestInfo.Browser,
      ClientLocation = clientRequestInfo.Location,
    };
  }

  public static ServiceHeader GetServiceHeader(this Controller controller, ClaimsPrincipal claimsPrincipal)
  {
    var systemInfo = new SystemInfo();
    var clientRequestInfo = controller.RetrieveClientRequestInfo() ?? new ClientRequestInfo();

    return new ServiceHeader
    {
      EnvironmentIPAddress = systemInfo.IPAddress,
      EnvironmentMACAddress = systemInfo.MACAddress,
      EnvironmentMotherboardSerialNumber = systemInfo.MotherboardSerialNumber,
      EnvironmentProcessorId = systemInfo.ProcessorId,
      EnvironmentUserName = systemInfo.UserName,
      EnvironmentMachineName = systemInfo.MachineName,
      EnvironmentDomainName = systemInfo.DomainName,
      EnvironmentOSVersion = Environment.OSVersion.ToString(),
      ApplicationUserName = claimsPrincipal.Identity?.Name,

      ClientIPAddress = clientRequestInfo.UserHostAddress,
      ClientSessionID = clientRequestInfo.SessionID,
      ClientUserHostAddress = clientRequestInfo.UserHostAddress,
      ClientUserAgent = clientRequestInfo.UserAgent,
      ClientUserHostName = clientRequestInfo.UserHostName,
      ClientBrowser = clientRequestInfo.Browser,
      ClientLocation = clientRequestInfo.Location,
    };
  }

  public static ServiceHeader GetServiceHeader(HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
  {
    var systemInfo = new SystemInfo();
    var clientRequestInfo = httpContext.RetrieveClientRequestInfo() ?? new ClientRequestInfo();

    return new ServiceHeader
    {
      EnvironmentIPAddress = systemInfo.IPAddress,
      EnvironmentMACAddress = systemInfo.MACAddress,
      EnvironmentMotherboardSerialNumber = systemInfo.MotherboardSerialNumber,
      EnvironmentProcessorId = systemInfo.ProcessorId,
      EnvironmentUserName = systemInfo.UserName,
      EnvironmentMachineName = systemInfo.MachineName,
      EnvironmentDomainName = systemInfo.DomainName,
      EnvironmentOSVersion = Environment.OSVersion.ToString(),
      ApplicationUserName = claimsPrincipal.Identity?.Name,

      ClientIPAddress = clientRequestInfo.UserHostAddress,
      ClientSessionID = clientRequestInfo.SessionID,
      ClientUserHostAddress = clientRequestInfo.UserHostAddress,
      ClientUserAgent = clientRequestInfo.UserAgent,
      ClientUserHostName = clientRequestInfo.UserHostName,
      ClientBrowser = clientRequestInfo.Browser,
      ClientLocation = clientRequestInfo.Location,
    };
  }

  public static ClientRequestInfo? RetrieveClientRequestInfo(this PageModel pageModel) => pageModel?.HttpContext.RetrieveClientRequestInfo();

  public static ClientRequestInfo? RetrieveClientRequestInfo(this Controller controller) => controller?.HttpContext.RetrieveClientRequestInfo();

  public static ClientRequestInfo? RetrieveClientRequestInfo(this HttpContext httpContext)
  {
    if (httpContext == null)
    {
      return null;
    }

    return new ClientRequestInfo()
    {
      //SessionID = httpContext.Session.IsAvailable ? httpContext.Session?.Id : string.Empty,
      UserHostAddress = httpContext.Connection.LocalIpAddress?.ToString(),
      UserHostName = httpContext.Connection.LocalIpAddress?.ToString(),
      UserAgent = httpContext.Request.Headers[HeaderNames.UserAgent],
      Browser = new BrowserInfo
      {
        Name = httpContext.Request.Headers[HeaderNames.UserAgent],
      }
    };
  }

  public static List<string>? ModelErrors(this Controller controller)
  {
    // Use LINQ to select the error messages from ModelState
    var errors = controller.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

    // Return the errors as a list
    return errors;
  }

  public static string? ModelErrorsAsString(this Controller controller)
  {
    // Use LINQ to select the error messages from ModelState
    var errors = controller.ModelErrors() != null && controller.ModelErrors()!.Count > 0 ? string.Join(",", controller.ModelErrors()!) : string.Empty;

    // Return the errors as a list
    return errors;
  }

  public static List<string>? ModelErrors(this PageModel pageModel)
  {
    // Use LINQ to select the error messages from ModelState
    var errors = pageModel.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

    // Return the errors as a list
    return errors;
  }

  public static string? ModelErrorsAsString(this PageModel pageModel)
  {
    // Use LINQ to select the error messages from ModelState
    var errors = pageModel.ModelErrors() != null && pageModel.ModelErrors()!.Count > 0 ? string.Join(",", pageModel.ModelErrors()!) : string.Empty;

    // Return the errors as a list
    return errors;
  }

}
