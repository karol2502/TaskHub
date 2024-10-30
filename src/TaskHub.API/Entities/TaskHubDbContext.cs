using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskHub.Entities;

public sealed class TaskHubDbContext(DbContextOptions<TaskHubDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<UserProject> UserProjects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskHubDbContext).Assembly);
    }
}
