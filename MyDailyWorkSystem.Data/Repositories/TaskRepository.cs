using Dapper;
using MyDailyWorkSystem.Data.Database;
using MyDailyWorkSystem.Domain.Enums;
using MyDailyWorkSystem.Domain.Models;
using System.Text.Json;

namespace MyDailyWorkSystem.Data.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(IDataConnectionFactory factory)
         : base(factory) { }

        public TaskItem InsertTask(TaskItem task)
        {
            if ( task.Id == Guid.Empty )
                task.Id = Guid.NewGuid();

            var tagJson = JsonSerializer.Serialize(task.TagIds);
            var attachmentJson = JsonSerializer.Serialize(task.AttachmentIds);

            using ( var connection = _factory.GetConnection() )
            {

                connection.Open();
                connection.Execute("""
                    INSERT INTO Tasks (
                        Id, CreatedAt, UpdatedAt, Title, Notes, Type, Priority, State,
                        DueDate, EstimatedHours, ActualFinishTime,
                        ProjectId, CreatorUserId, AssignedToUserId,
                        TagIds, AttachmentIds, OrderIndex
                    )
                    VALUES (
                        @Id, @CreatedAt, @UpdatedAt, @Title, @Notes, @Type, @Priority, @State,
                        @DueDate, @EstimatedHours, @ActualFinishTime,
                        @ProjectId, @CreatorUserId, @AssignedToUserId,
                        @TagIds, @AttachmentIds, @OrderIndex
                    );
                    """
                    , new
                    {
                        task.Id ,
                        task.CreatedAt ,
                        task.UpdatedAt ,
                        task.Title ,
                        task.Notes ,
                        task.Type ,
                        task.Priority ,
                        task.State ,
                        task.DueDate ,
                        task.EstimatedHours ,
                        task.ActualFinishTime ,
                        task.ProjectId ,
                        task.CreatorUserId ,
                        task.AssignedToUserId ,
                        TagIds = tagJson ,
                        AttachmentIds = attachmentJson ,
                        task.OrderIndex
                    });
                Console.WriteLine(
                    $"Inserted Task: {task.Id}, Title: {task.Title}"
                );
                return task;

            }
        }

        public void DeleteTask(Guid id)
        {
            using var connection = _factory.GetConnection();

            connection.Execute(
                "DELETE FROM Tasks WHERE Id = @Id;" ,
                new { Id = id });
        }

        public List<TaskItem> GetAllTasks()
        {
            using var connection = _factory.GetConnection();

            const string query = """
                SELECT Id, CreatedAt, UpdatedAt, Title, Notes, Type, Priority, State,
                       DueDate, EstimatedHours, ActualFinishTime,
                       ProjectId, CreatorUserId, AssignedToUserId,
                       TagIds as TagIdsJson, AttachmentIds as AttachmentIdsJson, OrderIndex
                FROM Tasks
                ORDER BY CreatedAt DESC;
                """;
            var tasks = connection.Query<TaskDto>(query);

            var list = tasks.ToList();
            return list.Select(DeserializeLists).ToList();
        }

        public TaskItem? GetTaskById(Guid id)
        {
            using var connection = _factory.GetConnection();

            const string query = """
                SELECT Id, CreatedAt, UpdatedAt, Title, Notes, Type, Priority, State,
                       DueDate, EstimatedHours, ActualFinishTime,
                       ProjectId, CreatorUserId, AssignedToUserId,
                       TagIds as TagIdsJson, AttachmentIds as AttachmentIdsJson, OrderIndex
                FROM Tasks
                WHERE Id = @Id;
                """;
            var dto = connection.QuerySingleOrDefault<TaskDto>(query , new { Id = id });
            if ( dto == null )
                return null;
            var task = DeserializeLists(dto);
            return task;
        }

        public void UpdateTask(TaskItem task)
        {
            task.UpdatedAt = DateTime.UtcNow;

            var tagJson = JsonSerializer.Serialize(task.TagIds ?? new List<Guid>());
            var attachmentJson = JsonSerializer.Serialize(task.AttachmentIds ?? new List<Guid>());

            using var connection = _factory.GetConnection();

            connection.Execute(
                """
                UPDATE Tasks
                SET UpdatedAt = @UpdatedAt,
                    Title = @Title,
                    Notes = @Notes,
                    Type = @Type,
                    Priority = @Priority,
                    State = @State,
                    DueDate = @DueDate,
                    EstimatedHours = @EstimatedHours,
                    ActualFinishTime = @ActualFinishTime,
                    ProjectId = @ProjectId,
                    CreatorUserId = @CreatorUserId,
                    AssignedToUserId = @AssignedToUserId,
                    TagIds = @TagIds,
                    AttachmentIds = @AttachmentIds,
                    OrderIndex = @OrderIndex
                WHERE Id = @Id;
                """ ,
                new
                {
                    task.Id ,
                    task.UpdatedAt ,
                    task.Title ,
                    task.Notes ,
                    task.Type ,
                    task.Priority ,
                    task.State ,
                    task.DueDate ,
                    task.EstimatedHours ,
                    task.ActualFinishTime ,
                    task.ProjectId ,
                    task.CreatorUserId ,
                    task.AssignedToUserId ,
                    TagIds = tagJson ,
                    AttachmentIds = attachmentJson ,
                    task.OrderIndex
                });
        }

        private static TaskItem DeserializeLists(TaskDto dto)
        {
            return new TaskItem
            {
                Id = Guid.Parse(dto.Id) ,
                CreatedAt = DateTime.Parse(dto.CreatedAt) ,
                UpdatedAt = DateTime.Parse(dto.UpdatedAt) ,
                Title = dto.Title ,
                Notes = dto.Notes ,
                Type = (TaskType)dto.Type ,
                Priority = (TaskPriority)dto.Priority ,
                State = (TaskState)dto.State ,
                DueDate = dto.DueDate == null ? null : DateTime.Parse(dto.DueDate) ,
                EstimatedHours = (uint?)dto.EstimatedHours ,
                ActualFinishTime = dto.ActualFinishTime == null ? null : DateTime.Parse(dto.ActualFinishTime) ,
                ProjectId = dto.ProjectId == null ? null : Guid.Parse(dto.ProjectId) ,
                CreatorUserId = dto.CreatorUserId == null ? null : Guid.Parse(dto.CreatorUserId) ,
                AssignedToUserId = dto.AssignedToUserId == null ? null : Guid.Parse(dto.AssignedToUserId) ,
                TagIds = dto.TagIds == null ? new() : JsonSerializer.Deserialize<List<Guid>>(dto.TagIds) ,
                AttachmentIds = dto.AttachmentIds == null ? new() : JsonSerializer.Deserialize<List<Guid>>(dto.AttachmentIds) ,
                OrderIndex = dto.OrderIndex
            };
        }

        private static List<Guid> DeserializeGuidList(string? json)
        {
            if ( string.IsNullOrWhiteSpace(json) )
            {
                return new List<Guid>();
            }

            try
            {
                var result = JsonSerializer.Deserialize<List<Guid>>(json);
                return result ?? new List<Guid>();
            }
            catch ( JsonException )
            {
                return new List<Guid>();
            }
        }
    }
}
