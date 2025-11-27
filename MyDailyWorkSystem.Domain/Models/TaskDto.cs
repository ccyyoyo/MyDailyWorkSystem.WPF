namespace MyDailyWorkSystem.Domain.Models
{
    public class TaskDto
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Title { get; set; }
        public string? Notes { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public int State { get; set; }
        public string? DueDate { get; set; }
        public int? EstimatedHours { get; set; }
        public string? ActualFinishTime { get; set; }
        public string? ProjectId { get; set; }
        public string? CreatorUserId { get; set; }
        public string? AssignedToUserId { get; set; }
        public string? TagIds { get; set; }
        public string? AttachmentIds { get; set; }
        public int OrderIndex { get; set; }
    }
}
