using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Core.Services
{
    public interface IProjectService
    {
        ProjectItem CreateProject(ProjectItem project);
        ProjectItem? GetProjectById(Guid id);
        List<ProjectItem> GetAllProjects();
        void UpdateProject(ProjectItem project);
        void DeleteProject(Guid id);
    }
}
