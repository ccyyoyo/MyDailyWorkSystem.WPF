using MyDailyWorkSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDailyWorkSystem.Domain.Models
{

    public class TaskItem
    {
        // Properties
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Task Details
        public string Title { get; set; }
        public string? Notes { get; set; }
        public TaskType Type { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskState State { get; set; }

        // Scheduling
        public DateTime? DueDate { get; set; }

        public uint? EstimatedHours { get; set; }

        public DateTime? ActualFinishTime { get; set; }

        public Guid? ProjectId { get; set; }
        public Guid? CreatorUserId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public List<Guid> TagIds { get; set; } = new();
        public List<Guid> AttachmentIds { get; set; } = new();
        public int OrderIndex { get; set; }
    }
}
