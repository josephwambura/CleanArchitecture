namespace Clean.Architecture.Application.DTO;

public class DashboardDTO
{
  public int? TotalProjects { get; set; }
  public int? TotalToDoItems { get; set; }
  public int? TotalCompleteToDoItems { get; set; }
  public int? TotalIncompleteToDoItems { get; set; }
}
