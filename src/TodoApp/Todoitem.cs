namespace TodoApp;

public class TodoItem
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public DateTimeOffset? DueDate { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public bool IsCompleted { get; private set; }

    public TodoItem(string title, DateTimeOffset? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        if (title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters.", nameof(title));

        CreatedAt = DateTimeOffset.UtcNow;
        if (dueDate.HasValue && dueDate.Value < CreatedAt)
            throw new ArgumentException("Due date cannot be before creation time.", nameof(dueDate));

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
        if (newTitle.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters.", nameof(newTitle));

        Title = newTitle;
    }

    public void ChangeDueDate(DateTimeOffset? newDueDate)
    {
        if (newDueDate.HasValue && newDueDate.Value < DateTimeOffset.UtcNow)
            throw new ArgumentException("Due date cannot be in the past.", nameof(newDueDate));
        DueDate = newDueDate;
    }

    public bool IsOverdue(DateTimeOffset? nowUtc = null)
    {
        var now = nowUtc ?? DateTimeOffset.UtcNow;
        if (!DueDate.HasValue)
            return false;
        return !IsCompleted && DueDate.Value < now;
    }

    public override string ToString() => $"{Title} (Due: {DueDate?.ToString("o") ?? "none"})";
    public override bool Equals(object? obj)
    {
        if (obj is not TodoItem other)
            return false;
        return Id == other.Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}