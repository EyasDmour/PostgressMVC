using PostMVC.Models;

namespace PostMVC.Data.Service;

public interface ITasksService
{
    Task<IEnumerable<Tasks>> GetAll();
    Task Add(Tasks task);
    Task Update(Tasks task);
    Task Delete(int id);
    Task<Tasks?> GetById(int id);
}