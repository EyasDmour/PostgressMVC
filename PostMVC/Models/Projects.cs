using System;
using System.ComponentModel.DataAnnotations;

namespace PostMVC.Models;

public class Projects
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; } = DateTime.Now;
    [Required]

    public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
    [Required]
    
    public int Priority { get; set; }
    public override string ToString()
    {
        return $"Project(Id={Id}, Name={Name}, Description={Description}, StartDate={StartDate}, EndDate={EndDate}, Priority={Priority})";
    }

}
