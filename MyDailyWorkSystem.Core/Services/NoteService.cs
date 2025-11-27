using MyDailyWorkSystem.Data.Repositories;
using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Core.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repo;

        public NoteService(INoteRepository repo)
        {
            _repo = repo;
        }

        // -------------------------
        // CREATE
        // -------------------------
        public NoteItem CreateNote(NoteItem note)
        {
            if ( note == null )
                throw new ArgumentNullException(nameof(note) , "Note cannot be null.");

            if ( string.IsNullOrWhiteSpace(note.Title) )
                throw new ArgumentException("Note title cannot be empty." , nameof(note.Title));

            if ( note.Id == Guid.Empty )
                note.Id = Guid.NewGuid();

            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = note.CreatedAt;

            EnsureCollectionsInitialized(note);

            return _repo.InsertNote(note);
        }

        // -------------------------
        // DELETE
        // -------------------------
        public void DeleteNote(Guid id)
        {
            if ( id == Guid.Empty )
                throw new ArgumentException("Note ID cannot be empty." , nameof(id));

            var existing = _repo.GetNoteById(id);
            if ( existing == null )
                throw new Exception($"Note with id {id} does not exist.");

            _repo.DeleteNote(id);
        }

        // -------------------------
        // GET LIST
        // -------------------------
        public List<NoteItem> GetAllNotes()
        {
            var notes = _repo.GetAllNotes();
            return notes ?? new List<NoteItem>();
        }

        // -------------------------
        // GET ONE
        // -------------------------
        public NoteItem? GetNoteById(Guid id)
        {
            if ( id == Guid.Empty )
                throw new ArgumentException("Note ID cannot be empty." , nameof(id));

            return _repo.GetNoteById(id);
        }

        // -------------------------
        // UPDATE
        // -------------------------
        public void UpdateNote(NoteItem note)
        {
            if ( note == null )
                throw new ArgumentNullException(nameof(note));

            if ( note.Id == Guid.Empty )
                throw new ArgumentException("Note ID cannot be empty." , nameof(note.Id));

            var existing = _repo.GetNoteById(note.Id);
            if ( existing == null )
                throw new Exception($"Note with id {note.Id} does not exist and cannot be updated.");

            note.UpdatedAt = DateTime.UtcNow;
            EnsureCollectionsInitialized(note);

            _repo.UpdateNote(note);
        }

        private static void EnsureCollectionsInitialized(NoteItem note)
        {
            note.RelatedProjectIds ??= new List<Guid>();
            note.RelatedTaskIds ??= new List<Guid>();
            note.RelatedPeople ??= new List<string>();
            note.TagIds ??= new List<Guid>();
            note.AttachmentIds ??= new List<Guid>();
        }
    }
}
