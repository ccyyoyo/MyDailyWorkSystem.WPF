using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Core.Services
{
    public interface ITaskService
    {
        TaskItem CreateTask(TaskItem task);
        TaskItem? GetTaskById(Guid id);
        List<TaskItem> GetAllTasks();
        void UpdateTask(TaskItem task);
        void DeleteTask(Guid id);

    }
}
