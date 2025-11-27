using Dapper;
using MyDailyWorkSystem.Data.Database;
using MyDailyWorkSystem.Data.Repositories.Dto;
using MyDailyWorkSystem.Domain.Models;
using System.Text.Json;

namespace MyDailyWorkSystem.Data.Repositories
{
    public class NoteRepository : BaseRepository, INoteRepository
    {
        public NoteRepository(IDataConnectionFactory factory) : base(factory) { }

        public NoteItem InsertNote(NoteItem n)
        {
            using var conn = GetConnection();

            conn.Execute("""
                INSERT INTO Notes (
                    Id, Title, Content, CreatedAt, UpdatedAt,
                    RelatedProjectIds, RelatedTaskIds, RelatedPeople,
                    TagIds, AttachmentIds,
                    OrderIndex
                )
                VALUES (
                    @Id, @Title, @Content, @CreatedAt, @UpdatedAt,
                    @RelatedProjectIds, @RelatedTaskIds, @RelatedPeople,
                    @TagIds, @AttachmentIds,
                    @OrderIndex
                );
                """ ,
                new
                {
                    Id = n.Id.ToString() ,
                    n.Title ,
                    n.Content ,
                    CreatedAt = n.CreatedAt.ToString("o") ,
                    UpdatedAt = n.UpdatedAt.ToString("o") ,

                    RelatedProjectIds = JsonSerializer.Serialize(n.RelatedProjectIds) ,
                    RelatedTaskIds = JsonSerializer.Serialize(n.RelatedTaskIds) ,
                    RelatedPeople = JsonSerializer.Serialize(n.RelatedPeople) ,

                    TagIds = JsonSerializer.Serialize(n.TagIds) ,
                    AttachmentIds = JsonSerializer.Serialize(n.AttachmentIds) ,

                    n.OrderIndex
                });

            return n;
        }

        public NoteItem? GetNoteById(Guid id)
        {
            using var conn = GetConnection();

            var dto = conn.QuerySingleOrDefault<NoteDto>(
                "SELECT * FROM Notes WHERE Id=@Id" ,
                new { Id = id.ToString() });

            return dto == null ? null : MapToNote(dto);
        }

        public List<NoteItem> GetAllNotes()
        {
            using var conn = GetConnection();

            var dtos = conn.Query<NoteDto>("SELECT * FROM Notes ORDER BY CreatedAt DESC").ToList();

            return dtos.Select(MapToNote).ToList();
        }

        public void UpdateNote(NoteItem n)
        {
            using var conn = GetConnection();

            conn.Execute("""
                UPDATE Notes SET
                    Title=@Title,
                    Content=@Content,
                    UpdatedAt=@UpdatedAt,
                    RelatedProjectIds=@RelatedProjectIds,
                    RelatedTaskIds=@RelatedTaskIds,
                    RelatedPeople=@RelatedPeople,
                    TagIds=@TagIds,
                    AttachmentIds=@AttachmentIds,
                    OrderIndex=@OrderIndex
                WHERE Id=@Id
                """ ,
                new
                {
                    n.Title ,
                    n.Content ,
                    UpdatedAt = n.UpdatedAt.ToString("o") ,
                    RelatedProjectIds = JsonSerializer.Serialize(n.RelatedProjectIds) ,
                    RelatedTaskIds = JsonSerializer.Serialize(n.RelatedTaskIds) ,
                    RelatedPeople = JsonSerializer.Serialize(n.RelatedPeople) ,
                    TagIds = JsonSerializer.Serialize(n.TagIds) ,
                    AttachmentIds = JsonSerializer.Serialize(n.AttachmentIds) ,
                    n.OrderIndex ,
                    Id = n.Id.ToString()
                });
        }

        public void DeleteNote(Guid id)
        {
            using var conn = GetConnection();
            conn.Execute("DELETE FROM Notes WHERE Id=@Id" , new { Id = id.ToString() });
        }

        private NoteItem MapToNote(NoteDto dto)
        {
            return new NoteItem
            {
                Id = Guid.Parse(dto.Id) ,
                Title = dto.Title ,
                Content = dto.Content ,
                CreatedAt = DateTime.Parse(dto.CreatedAt) ,
                UpdatedAt = DateTime.Parse(dto.UpdatedAt) ,

                RelatedProjectIds = JsonSerializer.Deserialize<List<Guid>>(dto.RelatedProjectIds ?? "[]")! ,
                RelatedTaskIds = JsonSerializer.Deserialize<List<Guid>>(dto.RelatedTaskIds ?? "[]")! ,
                RelatedPeople = JsonSerializer.Deserialize<List<Guid>>(dto.RelatedPeople ?? "[]")! ,

                TagIds = JsonSerializer.Deserialize<List<Guid>>(dto.TagIds ?? "[]")! ,
                AttachmentIds = JsonSerializer.Deserialize<List<Guid>>(dto.AttachmentIds ?? "[]")! ,

                OrderIndex = dto.OrderIndex
            };
        }
    }
}
