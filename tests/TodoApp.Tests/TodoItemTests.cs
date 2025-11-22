using System;
using TodoApp;
using Xunit;

namespace TodoApp.Tests;

public class TodoItemTests
{
    [Fact]
    public void Constructor_SetsTitleAndDueDate()
    {
        // Arrange
        var title = "Test item";
        var dueDate = DateTime.UtcNow.AddDays(1);

        // Act
        var item = new TodoItem(title, dueDate);

        // Assert
        Assert.Equal(title, item.Title);
        Assert.Equal(dueDate, item.DueDate);
        Assert.False(item.IsCompleted);
        Assert.NotEqual(Guid.Empty, item.Id);
    }

    [Fact]
    public void MarkComplete_SetsIsCompletedToTrue()
    {
        var item = new TodoItem("Test");

        item.MarkComplete();

        Assert.True(item.IsCompleted);
    }

    [Fact]
    public void ChangeTitle_UpdatesTitle()
    {
        var item = new TodoItem("Old title");

        item.ChangeTitle("New title");

        Assert.Equal("New title", item.Title);
    }

    [Fact]
    public void ChangeTitle_EmptyString_Throws()
    {
        var item = new TodoItem("Title");

        Assert.Throws<ArgumentException>(() => item.ChangeTitle(""));
    }

    [Fact]
    public void ChangeDueDate_UpdatesDueDate()
    {
        var item = new TodoItem("Test");
        var newDueDate = DateTime.UtcNow.AddDays(5);

        item.ChangeDueDate(newDueDate);

        Assert.Equal(newDueDate, item.DueDate);
    }

    [Fact]
    public void IsOverdue_NoDueDate_ReturnsFalse()
    {
        var item = new TodoItem("Test");

        var result = item.IsOverdue(DateTime.UtcNow);

        Assert.False(result);
    }

    [Fact]
    public void IsOverdue_NotCompletedAndPastDue_ReturnsTrue()
    {
        var past = DateTime.UtcNow.AddDays(-1);
        var now = DateTime.UtcNow;
        var item = new TodoItem("Test", past);

        var result = item.IsOverdue(now);

        Assert.True(result);
    }

    [Fact]
    public void IsOverdue_Completed_ReturnsFalse()
    {
        var past = DateTime.UtcNow.AddDays(-1);
        var now = DateTime.UtcNow;
        var item = new TodoItem("Test", past);
        item.MarkComplete();

        var result = item.IsOverdue(now);

        Assert.False(result);
    }
}
