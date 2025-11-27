using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyDailyWorkSystem.Core.Services;
using MyDailyWorkSystem.Data.Database;
using MyDailyWorkSystem.Data.Repositories;
using MyDailyWorkSystem.WPF.ViewModels;
using System.Windows;

namespace MyDailyWorkSystem.WPF
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_ , services) =>
                {
                    // ====== 註冊 Data 層 ======
                    services.AddSingleton<IDataConnectionFactory , SQLiteConnectionFactory>();
                    services.AddSingleton<DatabaseInitializer>();

                    services.AddSingleton<ITaskRepository , TaskRepository>();
                    services.AddSingleton<IProjectRepository , ProjectRepository>();
                    services.AddSingleton<INoteRepository , NoteRepository>();

                    // ====== 註冊 Service 層 ======
                    services.AddSingleton<ITaskService , TaskService>();
                    services.AddSingleton<IProjectService , ProjectService>();
                    services.AddSingleton<INoteService , NoteService>();

                    // ====== 註冊 ViewModels ======
                    services.AddTransient<TaskListViewModel>();

                    // ====== 註冊 Views ======
                    services.AddTransient<TaskListView>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost.Start();

            // 初始化資料庫
            using var scope = AppHost.Services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
            initializer.Initialize();

            base.OnStartup(e);
        }
    }
}
