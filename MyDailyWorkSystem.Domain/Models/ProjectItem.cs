using MyDailyWorkSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDailyWorkSystem.Domain.Models
{
    public class ProjectItem
    {
        // Identity & timestamps
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Basic info
        public string Name { get; set; }
        public string? ProjectCode { get; set; }
        public string? Description { get; set; }
        public ProjectType Type { get; set; }
        public ProjectStatus Status { get; set; }

        // Schedule / timing (all nullable)
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }

        // Management info
        public string? MainStakeholder { get; set; }
        public string? RiskNotes { get; set; }

        // Relations
        public List<Guid> TagIds { get; set; } = new();
        public List<Guid> RecordIds { get; set; } = new();
        public DateTime? LastRecordDate { get; set; }

        // Optional fields
        public int OrderIndex { get; set; }
    }
}
