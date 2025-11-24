using PostMVC.Models;

namespace PostMVC.Data.Service;

public interface IProjectsService
{
    Task<IEnumerable<Projects>> GetAll();
    Task Add(Projects project);
}
