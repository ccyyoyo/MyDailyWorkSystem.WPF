using Dapper;
using MyDailyWorkSystem.Data.Database;
using MyDailyWorkSystem.Data.Repositories.Dto;
using MyDailyWorkSystem.Domain.Enums;
using MyDailyWorkSystem.Domain.Models;
using System.Text.Json;

namespace MyDailyWorkSystem.Data.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(IDataConnectionFactory factory) : base(factory) { }

        public ProjectItem InsertProject(ProjectItem p)
        {
            using var conn = GetConnection();

            conn.Execute("""
                INSERT INTO Projects (
                    Id, CreatedAt, UpdatedAt, Name, Description,
                    Type, Status,
                    ExpectedStartDate, ExpectedEndDate, ActualEndDate,
                    MainStakeholder, RiskNotes,
                    TagIds, LastRecordDate,
                    OrderIndex
                )
                VALUES (
                    @Id, @CreatedAt, @UpdatedAt, @Name, @Description,
                    @Type, @Status,
                    @ExpectedStartDate, @ExpectedEndDate, @ActualEndDate,
                    @MainStakeholder, @RiskNotes,
                    @TagIds, @LastRecordDate,
                    @OrderIndex
                )
                """ ,
                new
                {
                    Id = p.Id.ToString() ,
                    CreatedAt = p.CreatedAt.ToString("o") ,
                    UpdatedAt = p.UpdatedAt.ToString("o") ,

                    p.Name ,
                    p.Description ,
                    Type = (int)p.Type ,
                    Status = (int)p.Status ,

                    ExpectedStartDate = p.ExpectedStartDate?.ToString("o") ,
                    ExpectedEndDate = p.ExpectedEndDate?.ToString("o") ,
                    ActualEndDate = p.ActualEndDate?.ToString("o") ,

                    p.MainStakeholder ,
                    p.RiskNotes ,

                    TagIds = JsonSerializer.Serialize(p.TagIds) ,
                    LastRecordDate = p.LastRecordDate?.ToString("o") ,
                    p.OrderIndex
                });

            return p;
        }

        public ProjectItem? GetProjectById(Guid id)
        {
            using var conn = GetConnection();

            var dto = conn.QuerySingleOrDefault<ProjectDto>(
                "SELECT * FROM Projects WHERE Id = @Id" ,
                new { Id = id.ToString() });

            return dto == null ? null : MapToProject(dto);
        }

        public List<ProjectItem> GetAllProjects()
        {
            using var conn = GetConnection();

            var dtos = conn.Query<ProjectDto>("SELECT * FROM Projects ORDER BY CreatedAt DESC").ToList();

            return dtos.Select(MapToProject).ToList();
        }

        public void UpdateProject(ProjectItem p)
        {
            using var conn = GetConnection();

            conn.Execute("""
                UPDATE Projects SET
                    UpdatedAt = @UpdatedAt,
                    Name = @Name,
                    Description = @Description,
                    Type = @Type,
                    Status = @Status,
                    ExpectedStartDate = @ExpectedStartDate,
                    ExpectedEndDate = @ExpectedEndDate,
                    ActualEndDate = @ActualEndDate,
                    MainStakeholder = @MainStakeholder,
                    RiskNotes = @RiskNotes,
                    TagIds = @TagIds,
                    LastRecordDate = @LastRecordDate,
                    OrderIndex = @OrderIndex
                WHERE Id = @Id
                """ ,
                new
                {
                    UpdatedAt = p.UpdatedAt.ToString("o") ,
                    p.Name ,
                    p.Description ,
                    Type = (int)p.Type ,
                    Status = (int)p.Status ,
                    ExpectedStartDate = p.ExpectedStartDate?.ToString("o") ,
                    ExpectedEndDate = p.ExpectedEndDate?.ToString("o") ,
                    ActualEndDate = p.ActualEndDate?.ToString("o") ,
                    p.MainStakeholder ,
                    p.RiskNotes ,
                    TagIds = JsonSerializer.Serialize(p.TagIds) ,
                    LastRecordDate = p.LastRecordDate?.ToString("o") ,
                    p.OrderIndex ,
                    Id = p.Id.ToString()
                });
        }

        public void DeleteProject(Guid id)
        {
            using var conn = GetConnection();
            conn.Execute("DELETE FROM Projects WHERE Id=@Id" , new { Id = id.ToString() });
        }

        private ProjectItem MapToProject(ProjectDto dto)
        {
            return new ProjectItem
            {
                Id = Guid.Parse(dto.Id) ,
                CreatedAt = DateTime.Parse(dto.CreatedAt) ,
                UpdatedAt = DateTime.Parse(dto.UpdatedAt) ,

                Name = dto.Name ,
                Description = dto.Description ,

                Type = (ProjectType)dto.Type ,
                Status = (ProjectStatus)dto.Status ,

                ExpectedStartDate = dto.ExpectedStartDate == null ? null : DateTime.Parse(dto.ExpectedStartDate) ,
                ExpectedEndDate = dto.ExpectedEndDate == null ? null : DateTime.Parse(dto.ExpectedEndDate) ,
                ActualEndDate = dto.ActualEndDate == null ? null : DateTime.Parse(dto.ActualEndDate) ,

                MainStakeholder = dto.MainStakeholder ,
                RiskNotes = dto.RiskNotes ,

                TagIds = string.IsNullOrWhiteSpace(dto.TagIds)
                    ? new List<Guid>()
                    : JsonSerializer.Deserialize<List<Guid>>(dto.TagIds) ,

                LastRecordDate = dto.LastRecordDate == null
                    ? null
                    : DateTime.Parse(dto.LastRecordDate) ,

                OrderIndex = dto.OrderIndex
            };
        }
    }
}
