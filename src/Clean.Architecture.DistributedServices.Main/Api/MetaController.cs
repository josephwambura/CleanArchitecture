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

    return Ok($"Company: {versionInfo.CompanyName}, Product: {versionInfo.ProductName}, Version: {versionInfo.ProductVersion}, Last Updated: {creationDate}");
  }
}
