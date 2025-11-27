using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Data.Repositories
{
    public interface INoteRepository
    {
        NoteItem InsertNote(NoteItem note);
        NoteItem? GetNoteById(Guid id);
        List<NoteItem> GetAllNotes();
        void UpdateNote(NoteItem note);
        void DeleteNote(Guid id);
    }
}
