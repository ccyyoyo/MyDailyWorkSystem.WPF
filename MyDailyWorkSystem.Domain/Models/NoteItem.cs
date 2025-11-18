using System;
using System.Collections.Generic;
using System.Text;

namespace MyDailyWorkSystem.Domain.Models
{
    public class NoteItem
    {
        // Identity & timestamps
        public Guid Id { get; set; }
        public string? SourceType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Core fields
        public string Title { get; set; }
        public string? Content { get; set; }

        // Relationships
        public List<Guid> RelatedProjectIds { get; set; } = new();
        public List<Guid> RelatedTaskIds { get; set; } = new();
        public List<String> RelatedPeople { get; set; } = new();

        // Additional metadata
        public List<Guid> TagIds { get; set; } = new();
        public List<Guid> AttachmentIds { get; set; } = new();
        public int OrderIndex { get; set; }
    }
}
