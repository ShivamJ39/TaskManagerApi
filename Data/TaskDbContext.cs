
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;




namespace TaskManagerApi.Data;
public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<Subtask> Subtasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskModel>()
            .HasMany(t => t.Subtasks)
            .WithOne(s => s.TaskModel!)
            .HasForeignKey(s => s.TaskModelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}



