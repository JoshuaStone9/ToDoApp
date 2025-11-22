using System;
using System.Linq;
using TodoApp;
using Xunit;

namespace TodoApp.Tests;

public class TodoListTests
{
    [Fact]
    public void Add_AddsNewItem()
    {
        var list = new TodoList();

        var item = list.Add("Test");

        Assert.Single(list.GetAll());
        Assert.Equal("Test", item.Title);
        Assert.Contains(item, list.GetAll());
    }

    [Fact]
    public void Remove_ExistingItem_ReturnsTrueAndRemoves()
    {
        var list = new TodoList();
        var item = list.Add("To remove");

        var result = list.Remove(item.Id);

        Assert.True(result);
        Assert.Empty(list.GetAll());
    }

    [Fact]
    public void Remove_NonExistingItem_ReturnsFalse()
    {
        var list = new TodoList();

        var result = list.Remove(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public void GetById_ReturnsCorrectItem()
    {
        var list = new TodoList();
        var item1 = list.Add("Item 1");
        var item2 = list.Add("Item 2");

        var result = list.GetById(item2.Id);

        Assert.Equal(item2.Id, result!.Id);
    }

    [Fact]
    public void GetById_NotFound_ReturnsNull()
    {
        var list = new TodoList();

        var result = list.GetById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public void GetActive_ReturnsOnlyNotCompleted()
    {
        var list = new TodoList();
        var item1 = list.Add("Active 1");
        var item2 = list.Add("Active 2");
        var completed = list.Add("Completed");
        completed.MarkComplete();

        var active = list.GetActive();

        Assert.Equal(2, active.Count);
        Assert.DoesNotContain(completed, active);
    }

    [Fact]
    public void GetCompleted_ReturnsOnlyCompleted()
    {
        var list = new TodoList();
        var item1 = list.Add("Item 1");
        var item2 = list.Add("Item 2");
        item2.MarkComplete();

        var completed = list.GetCompleted();

        Assert.Single(completed);
        Assert.Contains(item2, completed);
        Assert.DoesNotContain(item1, completed);
    }

    [Fact]
    public void GetOverdue_ReturnsOnlyOverdueItems()
    {
        var list = new TodoList();
        var now = DateTime.UtcNow;
        var overdue = list.Add("Overdue", now.AddDays(-1));
        var future = list.Add("Future", now.AddDays(1));
        var noDue = list.Add("No due date");
        var completedOverdue = list.Add("Completed overdue", now.AddDays(-2));
        completedOverdue.MarkComplete();

        var result = list.GetOverdue(now);

        Assert.Single(result);
        Assert.Contains(overdue, result);
        Assert.DoesNotContain(future, result);
        Assert.DoesNotContain(noDue, result);
        Assert.DoesNotContain(completedOverdue, result);
    }

    [Fact]
    public void ClearCompleted_RemovesCompletedItemsAndReturnsCount()
    {
        var list = new TodoList();
        var item1 = list.Add("Item 1");
        var item2 = list.Add("Item 2");
        var item3 = list.Add("Item 3");
        item1.MarkComplete();
        item3.MarkComplete();

        var removedCount = list.ClearCompleted();

        Assert.Equal(2, removedCount);
        Assert.Single(list.GetAll());
        Assert.Contains(item2, list.GetAll());
    }
}
