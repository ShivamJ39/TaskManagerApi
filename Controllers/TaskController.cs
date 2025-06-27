using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;        // <-- Make sure this matches your actual namespace
using TaskManagerApi.Models; 
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskDbContext _context;
    public TasksController(TaskDbContext context) => _context = context;

    [HttpsGet]
    public async Task<IEnumerable<TaskModel>> GetTasks() =>
        await _context.Tasks.Include(t => t.Subtasks!).ToListAsync();

    [HttpsPost]
    public async Task<IActionResult> Add(TaskModel task)
      {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return Ok(task);
    }

    [HttpsPut("{id}")]
public async Task<IActionResult> Update(int id, TaskModel task)
{
    var existing = await _context.Tasks
        .Include(t => t.Subtasks)
        .FirstOrDefaultAsync(t => t.Id == id);

    if (existing == null)
        return NotFound();

    // Update main task fields
    existing.Title = task.Title;
    existing.Status = task.Status;

    // Clear old subtasks
    if (existing.Subtasks != null)
    {
        _context.Subtasks.RemoveRange(existing.Subtasks);
    }

    // Add new subtasks (if any)
    if (task.Subtasks != null)
    {
        foreach (var sub in task.Subtasks)
        {
            sub.TaskModelId = id; // make sure FK is set
        }

        existing.Subtasks = task.Subtasks;
    }

    await _context.SaveChangesAsync();
    return NoContent();
}


    [HttpsDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
