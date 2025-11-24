using APIforPostMVC.Models;
using Microsoft.EntityFrameworkCore;
using APIforPostMVC.Data;

namespace APIforPostMVC.Data.Service;

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

    public async Task<Tasks?> GetById(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task Update(Tasks task, int userId)
    {
        var existingTask = await _context.Tasks.FindAsync(task.Id);
        if (existingTask == null) return;

        // Check access: Owner of project OR Member of project
        var project = await _context.Projects.FindAsync(existingTask.ProjectId);
        if (project == null) return;

        bool isOwner = project.OwnerId == userId;
        bool isMember = await _context.ProjectMembers.AnyAsync(pm => pm.ProjectId == project.Id && pm.UserId == userId);

        if (!isOwner && !isMember)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this task.");
        }

        existingTask.Title = task.Title;
        existingTask.Details = task.Details;
        existingTask.IsCompleted = task.IsCompleted;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id, int userId)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return;

        var project = await _context.Projects.FindAsync(task.ProjectId);
        if (project == null) return;

        if (project.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("Only the project owner can delete tasks.");
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}
