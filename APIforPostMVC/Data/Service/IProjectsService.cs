using APIforPostMVC.Models;

namespace APIforPostMVC.Data.Service;

public interface IProjectsService
{
    Task<IEnumerable<Projects>> GetAll(int userId);
    Task Add(Projects project);



}
