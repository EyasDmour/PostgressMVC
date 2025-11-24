using PostMVC.Models;

namespace PostMVC.Data.Service;

public interface ITasksService
{
    Task<IEnumerable<Tasks>> GetAll();
    Task Add(Tasks task);
}
