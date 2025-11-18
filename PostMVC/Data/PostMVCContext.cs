using Microsoft.EntityFrameworkCore;
using PostMVC.Models;

namespace PostMVC.Data;

public class PostMVCContext : DbContext
{
    public PostMVCContext (DbContextOptions<PostMVCContext> options)
        : base(options)
    {
    }
    public DbSet<Project> Project { get; set; } = default!; 
}
