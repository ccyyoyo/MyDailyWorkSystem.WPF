using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Core.Services
{
    internal interface ITaskService
    {
        TaskItem CreateTask(TaskItem task);
        TaskItem? GetTaskById(Guid id);
        List<TaskItem> GetAllTasks();
        void UpdateTask(TaskItem task);
        void DeleteTask(Guid id);

    }
}
