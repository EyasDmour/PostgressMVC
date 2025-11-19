using Microsoft.EntityFrameworkCore;
using PostMVC.Models;

namespace PostMVC.Data;

public class PostMVCContext : DbContext
{
    public PostMVCContext (DbContextOptions<PostMVCContext> options)
        : base(options)
    {
    }
    public DbSet<Projects> Projects { get; set; } = default!;
    public DbSet<Tasks> Tasks { get; set; } = default!;
}
