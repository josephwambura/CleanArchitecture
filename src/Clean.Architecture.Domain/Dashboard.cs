using Clean.Architecture.Domain.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain;

public class Dashboard : IAggregateRoot
{
  public static readonly string View =
    @$"CREATE VIEW View_{nameof(Dashboard)} AS
        SELECT 
          (SELECT Count(*) FROM {Utility.DbTableName<Project>()}) as Total{nameof(Project)}s,
          (SELECT Count(*) FROM {Utility.DbTableName<ToDoItem>()}) as Total{nameof(ToDoItem)}s,
          (SELECT Count(*) FROM {Utility.DbTableName<ToDoItem>()} i where i.IsDone = 1) as TotalComplete{nameof(ToDoItem)}s,
          (SELECT Count(*) FROM {Utility.DbTableName<ToDoItem>()} i where i.IsDone = 0) as TotalIncomplete{nameof(ToDoItem)}s";

  public int? TotalProjects { get; set; }
  public int? TotalToDoItems { get; set; }
  public int? TotalCompleteToDoItems { get; set; }
  public int? TotalIncompleteToDoItems { get; set; }
}
