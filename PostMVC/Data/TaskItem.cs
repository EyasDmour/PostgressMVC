using System.ComponentModel.DataAnnotations;

namespace PostMVC.Data;

public class TaskItem
{
    public int Id { get; set; }
    [Required]
    public required string Title { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}
