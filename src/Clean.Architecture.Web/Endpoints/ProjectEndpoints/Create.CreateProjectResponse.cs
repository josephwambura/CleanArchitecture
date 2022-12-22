namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class CreateProjectResponse
{
  public CreateProjectResponse(Guid id, string name)
  {
    Id = id;
    Name = name;
  }
  public Guid Id { get; set; }
  public string Name { get; set; }
}
