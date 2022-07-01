using Caliburn.Micro;
using System;
using static TaskManager.Models.Enums;

namespace TaskManager.Models
{
    public class Task : PropertyChangedBase
    {
        #region Fields
        private string _name;
        private string _description;
        private Status _status;
        private Priority _priority;
        private DateTime _dueDate;
        #endregion
        #region Properties
        public Guid Id { get; set; }
        public string Name 
        { 
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(nameof(Name)); } 
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyOfPropertyChange(nameof(Description)); }
        }
        public Status Status
        {
            get { return _status; }
            set { _status = value; NotifyOfPropertyChange(nameof(Status)); }
        }
        public Priority Priority
        {
            get { return _priority; }
            set { _priority = value; NotifyOfPropertyChange(nameof(Priority)); }
        }
        public DateTime CreatedOn { get; set; }
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; NotifyOfPropertyChange(nameof(DueDate)); }
        }
        public bool IsDeleted { get; set; } 
        #endregion

        public Task()
        {
            Id = Guid.NewGuid();
        }
        public Task(string name, string description, Status status, Priority priority, DateTime dueDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = string.IsNullOrWhiteSpace(description) ? "No description" : description;
            Status = status;
            Priority = priority;
            CreatedOn = DateTime.Now;
            DueDate = dueDate;
            IsDeleted = false;
        }

    }
}
