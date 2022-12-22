using System.ComponentModel.DataAnnotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class UpdateProjectRequest
{
  public const string Route = "/Projects";
  [Required]
  public Guid Id { get; set; }
  [Required]
  public string? Name { get; set; }
}
