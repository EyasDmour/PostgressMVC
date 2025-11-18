using System.ComponentModel.DataAnnotations;

namespace PostMVC.Data;

public class TaskItems
{
    public int Id { get; set; }
    [Required]
    public required string Title { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}
