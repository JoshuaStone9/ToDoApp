using TodoApp;

var todoList = new TodoList();

// Add some items
var now = DateTime.UtcNow;
var item1 = todoList.Add("Learn C#", now.AddDays(1));
var item2 = todoList.Add("Write unit tests", now.AddDays(-1)); // overdue
var item3 = todoList.Add("Refactor code"); // no due date

// Use TodoItem methods
item1.ChangeTitle("Learn advanced C#");
item3.ChangeDueDate(now.AddDays(2));
item1.MarkComplete();

// Use TodoList methods
Console.WriteLine("All items:");
foreach (var item in todoList.GetAll())
{
    Console.WriteLine($"- {item.Title} (Completed: {item.IsCompleted})");
}

Console.WriteLine();
Console.WriteLine("Active items:");
foreach (var item in todoList.GetActive())
{
    Console.WriteLine($"- {item.Title}");
}

Console.WriteLine();
Console.WriteLine("Completed items:");
foreach (var item in todoList.GetCompleted())
{
    Console.WriteLine($"- {item.Title}");
}

Console.WriteLine();
Console.WriteLine("Overdue items:");
foreach (var item in todoList.GetOverdue(now))
{
    Console.WriteLine($"- {item.Title}");
}


// Test Remove and GetById
var toRemoveId = item2.Id;
Console.WriteLine();
Console.WriteLine($"Removing item: {item2.Title}");
todoList.Remove(toRemoveId);
Console.WriteLine($"GetById after remove (should be null): {todoList.GetById(toRemoveId) is null}");

// Clear completed
var cleared = todoList.ClearCompleted();
Console.WriteLine();
Console.WriteLine($"Cleared {cleared} completed item(s).");
Console.WriteLine($"Remaining items: {todoList.GetAll().Count}");


bool menuActive = true;

while (menuActive)
{
    Console.WriteLine("\nMenu:");
    Console.WriteLine("=================================");
    Console.WriteLine("**GENERAL TODO LIST MENU**");
    Console.WriteLine("=================================");
    Console.WriteLine("1. Add new todo item");
    Console.WriteLine("2. View all items");
    Console.WriteLine("3. View active items");
    Console.WriteLine("4. View completed items");
    Console.WriteLine("5. View overdue items");
    Console.WriteLine("=================================");
    Console.WriteLine("**ITEM MANAGEMENT**");
    Console.WriteLine("=================================");
    Console.WriteLine("6. Delete item by ID");
    Console.WriteLine("7. Exit");
    Console.Write("Enter your choice: ");
    Console.WriteLine();
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("Enter the title of the new todo item:");
            var title = Console.ReadLine();
            var newItem = todoList.Add(title ?? "Untitled", now.AddDays(7));
            Console.WriteLine($"Added new item: {newItem.Title} with due date {newItem.DueDate}");
            break;

        case "2":
            Console.WriteLine("All todo items:");
            foreach (var item in todoList.GetAll())
                Console.WriteLine($"- {item.Title} (Completed: {item.IsCompleted})");
            break;

        case "3":
            Console.WriteLine("Active todo items:");
            foreach (var item in todoList.GetActive())
                Console.WriteLine($"- {item.Title}");
            break;

        case "4":
            Console.WriteLine("Completed todo items:");
            foreach (var item in todoList.GetCompleted())
                Console.WriteLine($"- {item.Title}");
            break;

        case "5":
            Console.WriteLine("Overdue todo items:");
            foreach (var item in todoList.GetOverdue(now))
                Console.WriteLine($"- {item.Title}");
            break;

        case "6":
            Console.WriteLine("Enter the title of the item to delete:");
            var deleteTitle = Console.ReadLine();

            var itemToDelete = todoList.GetAll()
            .FirstOrDefault(i => i.Title.Equals(deleteTitle, StringComparison.OrdinalIgnoreCase));

                if (itemToDelete != null)
                {
                    todoList.Remove(itemToDelete.Id);
                    Console.WriteLine("------Item removed successfully------");
                }
                else
                {
                    Console.WriteLine("Item not found.");
                }
                break;

        case "7":
            Console.WriteLine("Exiting...");
            menuActive = false;
            break;

        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}

Console.WriteLine();
Console.WriteLine("Thank you for using TodoApp!");






