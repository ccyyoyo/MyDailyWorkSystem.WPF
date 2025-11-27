namespace MyDailyWorkSystem.Data.Repositories.Dto
{
    public class NoteDto
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public string Title { get; set; }
        public string? Content { get; set; }

        public string? RelatedProjectIds { get; set; }
        public string? RelatedTaskIds { get; set; }
        public string? RelatedPeople { get; set; }
        public string? TagIds { get; set; }
        public string? AttachmentIds { get; set; }

        public int OrderIndex { get; set; }
    }
}
