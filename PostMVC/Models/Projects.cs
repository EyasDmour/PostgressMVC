using System;
using System.ComponentModel.DataAnnotations;

namespace PostMVC.Models;

public class Projects
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    [Required]

    public DateTime EndDate { get; set; } = DateTime.UtcNow.AddMonths(1);
    [Required]
    
    public int Priority { get; set; }
    
    public int OwnerId { get; set; }

    public override string ToString()
    {
        return $"Project(Id={Id}, Name={Name}, Description={Description}, StartDate={StartDate}, EndDate={EndDate}, Priority={Priority})";
    }

}
