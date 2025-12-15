# TodoApp

A small C# console app that helps you manage personal tasks from the terminal. Each todo tracks its own identifier, title, completion flag, and optional due date so you can stay on top of upcoming work without needing a UI framework or external service.

## Table of Contents
- [Overview](#overview)
- [High-Level Architecture](#high-level-architecture)
- [Data Model](#data-model)
- [Control Flow](#control-flow)
- [Plaid API Integration](#plaid-api-integration)
- [Local Persistence (todos.txt)](#local-persistence-todostxt)
- [Configuration & Environment](#configuration--environment)
- [How to Run](#how-to-run)
- [Potential Improvements](#potential-improvements)

## Overview
This project is a console-based productivity tool centered around simple todo items that need attention. Each item has:
* A human-friendly title.
* An auto-generated GUID so actions stay consistent across the session.
* An optional due date that keeps track of urgency.
* An `IsCompleted` flag to separate open work from what is done.

You can:

- View all current todos or filter by status (active, completed, overdue).
- Add new todos with a default due date of seven days out.
- Mark existing todos complete, edit titles, or delete items by title.
- Observe the current UTC timestamp at the top of each main-loop iteration for quick context.

Everything runs in-memory for fast iteration, making it a handy scaffold for experimenting with richer features later on.

## High-Level Architecture
All behavior lives inside the `TodoApp` namespace and is coordinated from `Program.cs`.

You can think of it roughly in layers:

### Presentation / UI Layer (Console)
`Program.cs` drives all console input/output, prints menu options, and routes user selections to the domain layer. The loop keeps running until the user selects “Exit”.

### Domain Logic (Todo List)
`TodoList.cs` acts as the domain service. It creates, queries, filters, and removes `TodoItem` instances. Filtering helpers such as `GetActive`, `GetCompleted`, and `GetOverdue` encapsulate the business rules.

### Persistence Layer (In-Memory Storage)
All items live in a single `List<TodoItem>` inside `TodoList`. While this session-only approach means you start fresh on every run, it avoids file or database dependencies and keeps the sample focused.

### External Integration Layer
There are currently no external API calls. The architecture is simple to extend, though; additional adapters or services can hook in without altering the menu surface area.

All state is encapsulated by:

```csharp
private readonly List<TodoItem> _items = new();
```

The list holds full `TodoItem` objects, so titles, due dates, and flags stay aligned by instance rather than index arithmetic.

## Data Model
The app uses a lightweight domain model with validation baked in:

### TodoItem
- `Guid Id` – unique identifier.
- `string Title` – guarded to be non-empty and ≤ 200 characters.
- `DateTimeOffset? DueDate` – optional; validated against current UTC time.
- `DateTimeOffset CreatedAt` – timestamp captured at creation.
- `bool IsCompleted` – toggled via `MarkComplete()`.

### TodoList
- Maintains the in-memory list of items.
- Offers query helpers such as `GetAll()`, `GetActive()`, `GetCompleted()`, and `GetOverdue(now)`.
- Handles mutations: `Add`, `Remove`, and `ClearCompleted`.

Example:

```csharp
var item = todoList.Add("Prep status report", DateTimeOffset.UtcNow.AddDays(3));
// item.Title == "Prep status report"
// item.IsCompleted == false
```

## Control Flow

### Program Startup
1. Instantiate a `TodoList`.
2. Display banner and the current UTC timestamp.
3. Enter the menu loop, prompting for user input on each iteration.

### Menu Options
1. **Add new todo item** – prompts for title, assigns due date of now + 7 days, and adds to the list.
2. **View all items** – prints every item with completion status.
3. **View active items** – filters to `!IsCompleted`.
4. **View completed items** – filters to `IsCompleted`.
5. **View overdue items** – uses `GetOverdue(now)` with the timestamp captured at loop start.
6. **Mark items completed** – looks up by title (case-insensitive) and toggles completion.
7. **Edit items** – fetches by title, then uses reflection to update the private `Title` setter (a quick-and-dirty approach that keeps the property encapsulated).
8. **Delete item by title** – removes the first matching item.
9. **Exit** – breaks the loop and thanks the user.

Invalid input simply re-prompts, keeping the session alive until an accepted option is chosen.

## Local Persistence (todos.txt)
The current implementation does not persist data to disk—items vanish once the process exits. If you want parity with the `todos.txt` concept:

- Serialize each `TodoItem` to lines like `Title|DueDate|Completed`.
- Load them at startup and rehydrate `TodoList`.
- Rewrite the file on any mutation to keep things atomic.

## Configuration & Environment
No environment variables are required. Running `dotnet run` inside `src/TodoApp` is enough because everything stays in memory. If you later introduce Plaid or another integration, a `.env` file would be the right place to stash secrets (`PLAID_CLIENT_ID`, `PLAID_SECRET`, etc.).

## How to Run

```bash
cd src/TodoApp
dotnet restore   # first-time setup
dotnet run
```

The app targets the standard .NET console runner, so it works cross-platform wherever the .NET SDK is available.

## Potential Improvements
- Persist todos to disk (JSON, LiteDB, SQLite) so lists survive app restarts.
- Replace the reflection-based title setter with explicit domain methods or DTOs.
- Allow editing due dates and completion status via dedicated menu entries.
- Add unit tests that cover List filtering (active/completed/overdue) and the CLI flow.
- Build a richer front end (web, UI toolkit, or Terminal.Gui) for easier navigation.
