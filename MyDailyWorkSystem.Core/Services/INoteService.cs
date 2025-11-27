using MyDailyWorkSystem.Domain.Models;

namespace MyDailyWorkSystem.Core.Services
{
    public interface INoteService
    {
        NoteItem CreateNote(NoteItem note);
        NoteItem? GetNoteById(Guid id);
        List<NoteItem> GetAllNotes();
        void UpdateNote(NoteItem note);
        void DeleteNote(Guid id);
    }
}
