using MyDailyWorkSystem.Data.Repositories;
using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo)
        {
            _repo = repo;
        }

        // -------------------------
        // CREATE
        // -------------------------
        public TaskItem CreateTask(TaskItem task)
        {
            if ( task == null )
                throw new ArgumentNullException(nameof(task) , "Task cannot be null.");

            if ( string.IsNullOrWhiteSpace(task.Title) )
                throw new ArgumentException("Task title cannot be empty." , nameof(task.Title));

            if ( task.Id == Guid.Empty )
                task.Id = Guid.NewGuid();

            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = task.CreatedAt;

            return _repo.InsertTask(task);
        }

        // -------------------------
        // DELETE
        // -------------------------
        public void DeleteTask(Guid id)
        {
            if ( id == Guid.Empty )
                throw new ArgumentException("Task ID cannot be empty." , nameof(id));

            var existing = _repo.GetTaskById(id);
            if ( existing == null )
                throw new Exception($"Task with id {id} does not exist.");

            _repo.DeleteTask(id);
        }

        // -------------------------
        // GET LIST
        // -------------------------
        public List<TaskItem> GetAllTasks()
        {
            var tasks = _repo.GetAllTasks();
            return tasks ?? new List<TaskItem>();
        }

        // -------------------------
        // GET ONE
        // -------------------------
        public TaskItem? GetTaskById(Guid id)
        {
            if ( id == Guid.Empty )
                throw new ArgumentException("Task ID cannot be empty." , nameof(id));

            return _repo.GetTaskById(id);
        }

        // -------------------------
        // UPDATE
        // -------------------------
        public void UpdateTask(TaskItem task)
        {
            if ( task == null )
                throw new ArgumentNullException(nameof(task));

            if ( task.Id == Guid.Empty )
                throw new ArgumentException("Task ID cannot be empty." , nameof(task.Id));

            var existing = _repo.GetTaskById(task.Id);
            if ( existing == null )
                throw new Exception($"Task with id {task.Id} does not exist and cannot be updated.");

            task.UpdatedAt = DateTime.UtcNow;

            _repo.UpdateTask(task);
        }
    }
}
