namespace Clean.Architecture.Application.DTO.ProjectAggregate;

public record ToDoItemRecord(Guid Id, string Title, string Description, bool IsDone);
