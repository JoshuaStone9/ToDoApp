namespace TodoApp;

public class TodoList
{
    private readonly List<TodoItem> _items = new();

    public IReadOnlyCollection<TodoItem> GetAll()
    {
        return _items.AsReadOnly();
    }

    public TodoItem Add(string title, DateTime? dueDate = null)
    {
        var item = new TodoItem(title, dueDate);
        _items.Add(item);
        return item;
    }

    public bool Remove(Guid id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null) return false;

        _items.Remove(item);
        return true;
    }

    public TodoItem? GetById(Guid id)
    {
        return _items.FirstOrDefault(i => i.Id == id);
    }

    public IReadOnlyCollection<TodoItem> GetActive()
    {
        return _items.Where(i => !i.IsCompleted).ToList();
    }

    public IReadOnlyCollection<TodoItem> GetCompleted()
    {
        return _items.Where(i => i.IsCompleted).ToList();
    }

    public IReadOnlyCollection<TodoItem> GetOverdue(DateTime nowUtc)
    {
        return _items.Where(i => i.IsOverdue(nowUtc)).ToList();
    }

    public int ClearCompleted()
    {
        var completed = _items.Where(i => i.IsCompleted).ToList();
        foreach (var item in completed)
        {
            _items.Remove(item);
        }

        return completed.Count;
    }
}
