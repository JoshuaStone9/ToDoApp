namespace TodoApp;

public class TodoItem
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public DateTime? DueDate { get; private set; }
    public bool IsCompleted { get; private set; }

    public TodoItem(string title, DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        Id = Guid.NewGuid();
        Title = title;
        DueDate = dueDate;
        IsCompleted = false;
    }

    public void MarkComplete()
    {
        IsCompleted = true;
    }

    public void ChangeTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title cannot be empty.", nameof(newTitle));

        Title = newTitle;
    }

    public void ChangeDueDate(DateTime? newDueDate)
    {
        DueDate = newDueDate;
    }

    public bool IsOverdue(DateTime nowUtc)
    {
        if (!DueDate.HasValue)
            return false;

        return !IsCompleted && DueDate.Value < nowUtc;
    }
}
