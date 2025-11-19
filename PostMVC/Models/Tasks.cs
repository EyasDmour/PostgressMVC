using System;

namespace PostMVC.Models;

public class Tasks
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    public int ProjectId { get; set; }
    public override string ToString()
    {
        return $"Task(Id={Id}, Title={Title}, Details={Details}, CreatedAt={CreatedAt}, IsCompleted={IsCompleted}, ProjectId={ProjectId})";
    }
}