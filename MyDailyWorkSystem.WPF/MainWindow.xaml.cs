using Microsoft.Extensions.DependencyInjection;
using MyDailyWorkSystem.Core.Services;
using MyDailyWorkSystem.Domain.Enums;
using MyDailyWorkSystem.Domain.Models;
using MyDailyWorkSystem.WPF.ViewModels;
using System.Windows;

namespace MyDailyWorkSystem.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            var taskService = App.AppHost.Services.GetRequiredService<ITaskService>();
            taskService.CreateTask(new TaskItem { Title = "測試任務" , Priority = TaskPriority.High });

            InitializeComponent();
        }

        private void OpenTasks_Click(object sender , RoutedEventArgs e)
        {
            var vm = App.AppHost.Services.GetRequiredService<TaskListViewModel>();
            vm.LoadTasks();

            var view = App.AppHost.Services.GetRequiredService<TaskListView>();
            view.DataContext = vm;

            MainContentArea.Content = view;
        }
    }
}
