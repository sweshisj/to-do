namespace Todo.api;

public record Todo
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
}

public record CreateTodoRequest
{
    public string Title { get; init; } = string.Empty;
}