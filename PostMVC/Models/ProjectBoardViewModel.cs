using PostMVC.Models;

namespace PostMVC.Models;

public class ProjectBoardViewModel
{
    public Projects Project { get; set; } = new();
    public IEnumerable<Tasks> Tasks { get; set; } = new List<Tasks>();
}
