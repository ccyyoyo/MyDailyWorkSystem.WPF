namespace MyDailyWorkSystem.Data.Repositories.Dto
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }

        public int Type { get; set; }
        public int Status { get; set; }

        public string? ExpectedStartDate { get; set; }
        public string? ExpectedEndDate { get; set; }
        public string? ActualEndDate { get; set; }

        public string? MainStakeholder { get; set; }
        public string? RiskNotes { get; set; }

        public string? TagIds { get; set; }
        public string? LastRecordDate { get; set; }

        public int OrderIndex { get; set; }
    }
}
