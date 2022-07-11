using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManager.Common;
using TaskManager.Models;
using static TaskManager.Models.Enums;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using TaskManager.Data;

namespace TaskManager.ViewModels
{
    public class HomeViewModel : Screen
    {
        #region Fields
        private DateTime _dueDate;
        private string _name;
        private Task _selectedTask;
        private string _description;
        private ObservableCollection<Task> _newTasks;
        private ObservableCollection<Task> _inProgressTasks;
        private ObservableCollection<Task> _completedTasks;
        private Priority _selectedPriority;
        private Status _selectedStatus;
        private string _submitBtnContent;
        private float _percentageComplete;
        private Category _selectedCategory;
        private readonly ITaskRepository _repository;
        #endregion
        #region Properties
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(nameof(Name));
                NotifyOfPropertyChange(nameof(CanCreateOrUpdateTask));
            }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyOfPropertyChange(nameof(Description)); }
        }
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                NotifyOfPropertyChange(nameof(SelectedStatus));
            }
        }
        public Priority SelectedPriority
        {
            get { return _selectedPriority; }
            set
            {
                _selectedPriority = value;
                NotifyOfPropertyChange(nameof(SelectedPriority));
            }
        }
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                NotifyOfPropertyChange(nameof(SelectedCategory));
            }
        }
        public DateTime DueDate
        {
            get
            {
                return _dueDate;
            }
            set
            {
                _dueDate = value;
                NotifyOfPropertyChange(nameof(DueDate));
            }
        }
        public float PercentageComplete
        {
            get { return _percentageComplete; }
            set
            {
                _percentageComplete = value;
                if (value == 100)
                    SelectedStatus = Status.Completed;
                NotifyOfPropertyChange(nameof(PercentageComplete));
            }
        }
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set { _selectedTask = value; NotifyOfPropertyChange(nameof(SelectedTask)); }
        }
        public ObservableCollection<Task> NewTasks
        {
            get { return _newTasks; }
            set
            {
                _newTasks = value;
                NotifyOfPropertyChange(nameof(NewTasks));
            }

        }
        public ObservableCollection<Task> InProgressTasks
        {
            get { return _inProgressTasks; }
            set
            {
                _inProgressTasks = value;
                NotifyOfPropertyChange(nameof(InProgressTasks));
            }
        }
        public ObservableCollection<Task> CompletedTasks
        {
            get { return _completedTasks; }
            set
            {
                _completedTasks = value;
                NotifyOfPropertyChange(nameof(CompletedTasks));
            }
        }
        public IEnumerable<Status> StatusOptions
        {
            get
            {
                return Enum.GetValues(typeof(Status)).Cast<Status>();
            }
        }
        public bool CanCreateOrUpdateTask
        {
            get { return !string.IsNullOrWhiteSpace(Name); }
        }
        public string SubmitBtnContent
        {
            get { return _submitBtnContent; }
            set { _submitBtnContent = value; NotifyOfPropertyChange(nameof(SubmitBtnContent)); }
        }

        #endregion

        public HomeViewModel(SqliteRepository sqliteRepository, SqlServerRepository sqlServerRepository)
        {
            _repository = Application.Current.Properties[Constant.Database].ToString() == Constant.Sqlite ? sqliteRepository : sqlServerRepository;
            InitializeTaskLists();
            Name = string.Empty;
            Description = string.Empty;

        private void InitializeTaskLists()
        {
            NewTasks = new(_repository.GetAllTasks().Where(tsk => tsk.Status == Status.New));
            InProgressTasks = new(_repository.GetAllTasks().Where(tsk => tsk.Status == Status.InProgress));
            CompletedTasks = new(_repository.GetAllTasks().Where(tsk => tsk.Status == Status.Completed));
        }

        public void CreateOrUpdateTask()
        {
            if (SelectedStatus == Status.New)
                NewTasks.Add(new(Name, Description, SelectedStatus, SelectedPriority, DueDate));
            else if (SelectedStatus == Status.InProgress)
                InProgressTasks.Add(new(Name, Description, SelectedStatus, SelectedPriority, DueDate));
            else
                UpdateTask();
            ResetInputControls();
        }

        public void CreateTask()
        {
            Task task = new(Name, Description, SelectedStatus, SelectedPriority, DueDate, SelectedCategory, PercentageComplete);
            _repository.CreateTask(task);
            switch (task.Status)
            {
                case Status.New:
                    NewTasks.Add(task);
                    break;
                case Status.InProgress:
                    InProgressTasks.Add(task);
                    break;
                case Status.Completed:
                    CompletedTasks.Add(task);
                    break;
            }
        }

        public void UpdateTask()
        {
            SelectedTask.Name = Name;
            SelectedTask.Description = Description;
            SelectedTask.Status = SelectedStatus;
            SelectedTask.Priority = SelectedPriority;
            SelectedTask.Category = SelectedCategory;
            SelectedTask.PercentageCompleted = PercentageComplete;
            SelectedTask.DueDate = DueDate;
            _repository.UpdateTask(SelectedTask);
            InitializeTaskLists();
        }

        public void ResetInputControls()
        {
            ResetInputControls();
        }

        public void MouseMoveHandler(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.Source is ListBox lbox && lbox.SelectedItem != null)
            {
                DragDrop.DoDragDrop(lbox, lbox.SelectedItem, DragDropEffects.Move);
            }
        }

        public void DropOnNewTasks(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Task)) is Task task && task.Status != Status.New)
            {
                RemoveTaskFromDragSource(task);
                task.Status = Status.New;
                _repository.UpdateTask(task);
                NewTasks.Add(task);
            }
        }

        public void DropOnInProgressTasks(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Task)) is Task task && task.Status != Status.InProgress)
            {
                RemoveTaskFromDragSource(task);
                task.Status = Status.InProgress;
                _repository.UpdateTask(task);
                InProgressTasks.Add(task);
            }
        }

        public void DropOnCompletedTasks(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Task)) is Task task && task.Status != Status.Completed)
            {
                RemoveTaskFromDragSource(task);
                task.Status = Status.Completed;
                _repository.UpdateTask(task);
                CompletedTasks.Add(task);
            }
        }

        private void RemoveTaskFromDragSource(Task task)
        {
            switch (task.Status)
            {
                case Status.New:
                    NewTasks.Remove(task);
                    break;
                case Status.InProgress:
                    InProgressTasks.Remove(task);
                    break;
                case Status.Completed:
                    CompletedTasks.Remove(task);
                    break;
            }
        }

    }
}
