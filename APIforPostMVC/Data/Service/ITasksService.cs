using APIforPostMVC.Models;

namespace APIforPostMVC.Data.Service;

public interface ITasksService
{
    Task<IEnumerable<Tasks>> GetAll();
    Task Add(Tasks task);
    Task<Tasks?> GetById(int id);
    Task Update(Tasks task, int userId);
    Task Delete(int id, int userId);
}
