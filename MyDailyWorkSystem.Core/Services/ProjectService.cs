using MyDailyWorkSystem.Data.Repositories;
using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repo;

        public ProjectService(IProjectRepository repo)
        {
            _repo = repo;
        }

        // -------------------------
        // CREATE
        // -------------------------
        public ProjectItem CreateProject(ProjectItem project)
        {
            if ( project == null )
                throw new ArgumentNullException(nameof(project) , "Project cannot be null.");

            if ( string.IsNullOrWhiteSpace(project.Name) )
                throw new ArgumentException("Project name cannot be empty." , nameof(project.Name));

            if ( project.Id == Guid.Empty )
                project.Id = Guid.NewGuid();

            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = project.CreatedAt;

            return _repo.InsertProject(project);
        }

        // -------------------------
        // DELETE
        // -------------------------
        public void DeleteProject(Guid id)
        {
            if ( id == Guid.Empty )
                throw new ArgumentException("Project ID cannot be empty." , nameof(id));

            var existing = _repo.GetProjectById(id);
            if ( existing == null )
                throw new Exception($"Project with id {id} does not exist.");

            _repo.DeleteProject(id);
        }

        // -------------------------
        // GET LIST
        // -------------------------
        public List<ProjectItem> GetAllProjects()
        {
            var projects = _repo.GetAllProjects();
            return projects ?? new List<ProjectItem>();
        }

        // -------------------------
        // GET ONE
        // -------------------------
        public ProjectItem? GetProjectById(Guid id)
        {
            if ( id == Guid.Empty )
                throw new ArgumentException("Project ID cannot be empty." , nameof(id));

            return _repo.GetProjectById(id);
        }

        // -------------------------
        // UPDATE
        // -------------------------
        public void UpdateProject(ProjectItem project)
        {
            if ( project == null )
                throw new ArgumentNullException(nameof(project));

            if ( project.Id == Guid.Empty )
                throw new ArgumentException("Project ID cannot be empty." , nameof(project.Id));

            var existing = _repo.GetProjectById(project.Id);
            if ( existing == null )
                throw new Exception($"Project with id {project.Id} does not exist and cannot be updated.");

            project.UpdatedAt = DateTime.UtcNow;

            _repo.UpdateProject(project);
        }
    }
}
