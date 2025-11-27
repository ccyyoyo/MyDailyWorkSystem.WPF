using MyDailyWorkSystem.Core.Services;
using MyDailyWorkSystem.Domain.Models;
using System.Collections.ObjectModel;

namespace MyDailyWorkSystem.WPF.ViewModels
{
    public class TaskListViewModel
    {
        private readonly ITaskService _taskService;

        public ObservableCollection<TaskItem> Tasks { get; set; }

        public TaskListViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<TaskItem>();
        }

        public void LoadTasks()
        {
            var items = _taskService.GetAllTasks();

            Tasks.Clear();
            foreach ( var t in items )
                Tasks.Add(t);
        }
    }

}
