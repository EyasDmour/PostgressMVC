using APIforPostMVC.Models;

namespace APIforPostMVC.Data.Service;

public interface IProjectsService
{
    Task<IEnumerable<Projects>> GetAll(int userId);
    Task Add(Projects project);
    Task<Projects?> GetById(int id);
    Task Delete(int id, int userId);
    Task AddMember(int projectId, string username, int ownerId);


}
