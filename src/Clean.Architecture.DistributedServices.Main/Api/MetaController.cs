using System.Diagnostics;

namespace Clean.Architecture.DistributedServices.Main.Api;

public class MetaController : BaseApiController
{
  [HttpGet("/info")]
  public ActionResult<string> Info()
  {
    var assembly = typeof(Program).Assembly;

    var creationDate = System.IO.File.GetCreationTime(assembly.Location);
    var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
    var version = versionInfo.ProductVersion;
    var productName = versionInfo.ProductName;
    var companyName = versionInfo.CompanyName;

    return Ok($"Company: {companyName}, Product: {productName}, Version: {version}, Last Updated: {creationDate}");
  }
}
