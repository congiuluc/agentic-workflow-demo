using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class TodoRepository(TodoDbContext context) : ITodoRepository
{
    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await context.TodoItems
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await context.TodoItems.FindAsync(id);
    }

    public async Task<TodoItem> CreateAsync(TodoItem item)
    {
        context.TodoItems.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<TodoItem?> UpdateAsync(int id, TodoItem item)
    {
        var existing = await context.TodoItems.FindAsync(id);
        if (existing is null) return null;

        existing.Title = item.Title;
        existing.Description = item.Description;
        existing.IsCompleted = item.IsCompleted;
        existing.DueDate = item.DueDate;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await context.TodoItems.FindAsync(id);
        if (item is null) return false;

        context.TodoItems.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }
}
