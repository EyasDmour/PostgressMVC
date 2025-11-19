using PostMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace PostMVC.Data.Service;

public class TasksService : ITasksService
{
    private readonly PostMVCContext _context;

    public TasksService(PostMVCContext context)
    {
        _context = context;
    }
    public async Task Add(Tasks task)
    {
        task.CreatedAt = DateTime.SpecifyKind(task.CreatedAt, DateTimeKind.Utc);

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tasks>> GetAll()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return tasks;
    }
}
