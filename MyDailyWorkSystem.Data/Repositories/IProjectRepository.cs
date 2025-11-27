using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Data.Repositories
{
    public interface IProjectRepository
    {
        ProjectItem InsertProject(ProjectItem project);

        ProjectItem? GetProjectById(Guid id);

        List<ProjectItem> GetAllProjects();

        void UpdateProject(ProjectItem project);

        void DeleteProject(Guid id);
    }
}
