using TodoApp;

var todoList = new TodoList();
var now = DateTime.UtcNow;

bool menuActive = true;

while (menuActive)
{
    Console.WriteLine("--------------------------------");
    Console.WriteLine("WELCOME TO THE TO DO APP!");
    Console.WriteLine("--------------------------------");
    Console.WriteLine("");
    Console.WriteLine("Current UTC Time: " + DateTime.UtcNow);
    Console.WriteLine("");
    Console.WriteLine("\nMenu:");
    Console.WriteLine("=================================");
    Console.WriteLine("**GENERAL TO DO LIST MENU**");
    Console.WriteLine("=================================");
    Console.WriteLine("1. Add new todo item");
    Console.WriteLine("2. View all items");
    Console.WriteLine("3. View active items");
    Console.WriteLine("4. View completed items");
    Console.WriteLine("5. View overdue items");
    Console.WriteLine("=================================");
    Console.WriteLine("**ITEM MANAGEMENT**");
    Console.WriteLine("=================================");
    Console.WriteLine("6. Mark Items Completed");
    Console.WriteLine("7. Edit items");
    Console.WriteLine("8. Delete item by Title");
    Console.WriteLine("9. Exit");
    Console.WriteLine("=================================");
    Console.WriteLine("Enter your choice: ");
    string? choice = Console.ReadLine();
    Console.WriteLine("=================================");
    Console.WriteLine();


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
            Console.WriteLine("Enter the title of the item to mark as completed:");
            var completeTitle = Console.ReadLine();

            var itemToComplete = todoList.GetAll()
                .FirstOrDefault(i => 
                    i.Title.Equals(completeTitle, StringComparison.OrdinalIgnoreCase));

            if (itemToComplete != null)
            {
                itemToComplete.MarkComplete();
                Console.WriteLine("------Item marked as completed------");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
            break;

        case "7":
            Console.WriteLine("Enter the title of the item to edit:");
            var editTitle = Console.ReadLine();

            var itemToEdit = todoList.GetAll()
                .FirstOrDefault(i =>
                    i.Title.Equals(editTitle, StringComparison.OrdinalIgnoreCase));

            if (itemToEdit != null)
            {
                Console.WriteLine("Enter the new title (leave empty to keep current):");
                var newTitle = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    var prop = itemToEdit.GetType().GetProperty("Title", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    var setMethod = prop?.GetSetMethod(true);
                    if (setMethod != null)
                    {
                        setMethod.Invoke(itemToEdit, new object?[] { newTitle });
                    }
                    else
                    {
                        Console.WriteLine("Unable to update Title: setter is not accessible.");
                    }
                }
                Console.WriteLine("------Item updated successfully------");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
            break;

        case "8":
            Console.WriteLine("Enter the title of the item to delete:");
            var deleteTitle = Console.ReadLine();

            var itemToDelete = todoList.GetAll()
                .FirstOrDefault(i => 
                    i.Title.Equals(deleteTitle, StringComparison.OrdinalIgnoreCase));

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

        case "9":
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
