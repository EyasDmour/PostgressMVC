using APIforPostMVC.Models;
using Microsoft.EntityFrameworkCore;
using APIforPostMVC.Data;

namespace APIforPostMVC.Data.Service;

public class ProjectsService : IProjectsService
{
    private readonly PostMVCContext _context;

    public ProjectsService(PostMVCContext context)
    {
        _context = context;
    }
    public async Task Add(Projects project)
    {
        project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
        project.EndDate   = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Projects>> GetAll(int userId)
    {
        // Get projects owned by user OR where user is a member
        var projects = await _context.Projects
            .Include(p => p.Owner)
            .Where(p => p.OwnerId == userId || _context.ProjectMembers.Any(pm => pm.ProjectId == p.Id && pm.UserId == userId))
            .ToListAsync();
        return projects;
    }

    public async Task<Projects?> GetById(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task Delete(int id, int userId)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null && project.OwnerId == userId)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
        else if (project != null)
        {
            throw new UnauthorizedAccessException("Only the owner can delete this project.");
        }
    }

    public async Task AddMember(int projectId, string username, int ownerId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) throw new Exception("Project not found");
        
        if (project.OwnerId != ownerId)
        {
            throw new UnauthorizedAccessException("Only the owner can add members.");
        }

        var userToAdd = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (userToAdd == null) throw new Exception("User not found");

        if (await _context.ProjectMembers.AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userToAdd.Id))
        {
            return; // Already a member
        }

        var member = new ProjectMember
        {
            ProjectId = projectId,
            UserId = userToAdd.Id,
            ProjectRole = "Member"
        };

        _context.ProjectMembers.Add(member);
        await _context.SaveChangesAsync();
    }
}
