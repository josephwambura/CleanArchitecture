
namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class GetProjectByIdRequest
{
  public const string Route = "/Projects/{ProjectId:Guid}";
  public static string BuildRoute(Guid projectId) => Route.Replace("{ProjectId:Guid}", projectId.ToString());

  public Guid ProjectId { get; set; }
}
