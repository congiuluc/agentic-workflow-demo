using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public class UpdateTodoRequest
{
    [Required]
    [MaxLength(200)]
    public required string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; }
}
