using MyDailyWorkSystem.Data.Repositories;
using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Core.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo)
        {
            _repo = repo;
        }
        public TaskItem CreateTask(TaskItem task)
        {
            if ( task == null )
                throw new ArgumentNullException("task" , "Task cannot be null.");

            if ( string.IsNullOrWhiteSpace(task.Title) )
            {
                throw new ArgumentException("Task title cannot be empty." , nameof(task.Title));
            }
            if ( task.Id == Guid.Empty )
            {
                task.Id = Guid.NewGuid();
            }

            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = task.CreatedAt;

            var createdTask = _repo.InsertTask(task);
            if ( createdTask == null )
            {
                throw new Exception("Failed to create task.");
            }
            return createdTask;
        }
    }
}