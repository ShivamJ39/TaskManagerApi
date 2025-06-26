namespace TaskManagerApi.Models {
public class TaskModel
{
    public int Id { get; set; }
    
    public string? Title { get; set; }

        public string? Status { get; set; } // "active", "completed", etc.
    public List<Subtask>? Subtasks { get; set; }
}

public class Subtask
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool Completed { get; set; }

    public int TaskModelId { get; set; } // Foreign key
    public TaskModel? TaskModel { get; set; }
}
}
