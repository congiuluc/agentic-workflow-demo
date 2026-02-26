using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todos").WithTags("Todos");

        group.MapGet("/", async (ITodoRepository repository) =>
        {
            var items = await repository.GetAllAsync();
            return Results.Ok(items);
        });

        group.MapGet("/{id:int}", async (int id, ITodoRepository repository) =>
        {
            var item = await repository.GetByIdAsync(id);
            return item is null ? Results.NotFound() : Results.Ok(item);
        })
        .WithName("GetTodoById");

        group.MapPost("/", async (CreateTodoRequest request, ITodoRepository repository) =>
        {
            var item = new TodoItem
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate
            };

            var created = await repository.CreateAsync(item);
            return Results.CreatedAtRoute("GetTodoById", new { id = created.Id }, created);
        });

        group.MapPut("/{id:int}", async (int id, UpdateTodoRequest request, ITodoRepository repository) =>
        {
            var item = new TodoItem
            {
                Title = request.Title,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
                DueDate = request.DueDate
            };

            var updated = await repository.UpdateAsync(id, item);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:int}", async (int id, ITodoRepository repository) =>
        {
            var deleted = await repository.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
