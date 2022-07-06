﻿using Caliburn.Micro;
using System;
using static TaskManager.Models.Enums;

namespace TaskManager.Models
{
    public class TaskDisplayModel : PropertyChangedBase
    {
        #region Fields
        private string _name;
        private string _description;
        private Status _status;
        private Priority _priority;
        private Category _category;
        private float _percentageCompleted;
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
        public Category Category 
        {
            get { return _category; }
            set { _category = value; NotifyOfPropertyChange(nameof(Category)); }
        }
        public float PercentageCompleted 
        { 
            get { return _percentageCompleted; }
            set { _percentageCompleted = value; NotifyOfPropertyChange(nameof(PercentageCompleted)); }
        }
        public DateTime CreatedOn { get; set; }
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; NotifyOfPropertyChange(nameof(DueDate)); }
        }
        public bool IsDeleted { get; set; }
        #endregion

        public TaskDisplayModel()
        {
            Id = Guid.NewGuid();
        }
        public TaskDisplayModel(string name, string description, Status status, Priority priority, DateTime dueDate,Category taskCategory, float percentageCompleted)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Status = status;
            Priority = priority;
            CreatedOn = DateTime.Now;
            Category = taskCategory;
            PercentageCompleted = percentageCompleted;
            DueDate = dueDate;
            IsDeleted = false;
        }

    }
}