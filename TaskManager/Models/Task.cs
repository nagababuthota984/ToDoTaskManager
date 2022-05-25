using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaskManager.Models.Enums;

namespace TaskManager.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsDeleted { get; set; }

        public Task()
        {
            Id = Guid.NewGuid();
        }
        public Task(string name, string description, Status status, Priority priority,DateTime dueDate)        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Status = status;
            Priority = priority;
            CreatedOn = DateTime.Now;
            DueDate = dueDate;
            IsDeleted = false;
        }

    }
}
