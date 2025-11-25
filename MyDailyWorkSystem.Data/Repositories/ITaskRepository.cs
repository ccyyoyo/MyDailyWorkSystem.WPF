using MyDailyWorkSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDailyWorkSystem.Data.Repositories
{
    public interface ITaskRepository
    {
        public abstract TaskItem InsertTask(TaskItem task);
        public abstract TaskItem? GetTaskById(Guid id);
        public abstract List<TaskItem> GetAllTasks();
        public abstract void UpdateTask(TaskItem task);
        public abstract void DeleteTask(Guid id);


    }
}
