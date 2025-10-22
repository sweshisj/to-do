using System.Collections.Concurrent;
using System.Threading;

namespace Todo.api;

public interface ITodoStore
{
    IReadOnlyCollection<Todo> GetAll();
    Todo Add(string title);
    bool Delete(int id);
}

public sealed class InMemoryTodoStore : ITodoStore
{
    private readonly ConcurrentDictionary<int, Todo> _todos = new();
    private int _nextId = 0;

    public IReadOnlyCollection<Todo> GetAll() => _todos.Values.OrderBy(t => t.Id).ToArray();

    public Todo Add(string title)
    {
        var id = Interlocked.Increment(ref _nextId);
        var todo = new Todo { Id = id, Title = title, IsCompleted = false };
        _todos[id] = todo;
        return todo;
    }

    public bool Delete(int id) => _todos.TryRemove(id, out _);
}

