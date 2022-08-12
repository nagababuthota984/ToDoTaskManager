using System;
using System.Collections.Generic;

namespace TaskManager.RemoteData
{
    public partial class Task
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? Status { get; set; }
        public int? Priority { get; set; }
        public int? Category { get; set; }
        public decimal? PercentageCompleted { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
