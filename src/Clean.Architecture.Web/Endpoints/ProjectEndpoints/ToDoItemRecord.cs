namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public record ToDoItemRecord(Guid Id, string Title, string Description, bool IsDone);
