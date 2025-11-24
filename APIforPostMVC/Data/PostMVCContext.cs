using Microsoft.EntityFrameworkCore;
using APIforPostMVC.Models;

namespace APIforPostMVC.Data;

public class PostMVCContext : DbContext
{
    public PostMVCContext (DbContextOptions<PostMVCContext> options)
        : base(options)
    {
    }
    public DbSet<Projects> Projects { get; set; } = default!;
    public DbSet<Tasks> Tasks { get; set; } = default!;
    public DbSet<Users> Users { get; set; } = default!;
    public DbSet<ProjectMember> ProjectMembers { get; set; } = default!;
}
