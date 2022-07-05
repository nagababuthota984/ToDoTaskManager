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
        private IDialogCoordinator _dialogCoordinator;
        #endregion

        #region Properties
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
        public Priority SelectedPriority
        {
            get { return _selectedPriority; }
            set
            {
                _selectedPriority = value;
                NotifyOfPropertyChange(nameof(SelectedPriority));
            }
        }
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                if (SelectedStatus == Status.Completed && PercentageComplete != 100)
                    PercentageComplete = 100;
                NotifyOfPropertyChange(nameof(SelectedStatus));
            }
        }
        public IEnumerable<Status> StatusOptions
        {
            get
            {
                return Enum.GetValues(typeof(Status)).Cast<Status>();
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
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set { _selectedTask = value; NotifyOfPropertyChange(nameof(SelectedTask)); }
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
        //acts similar to a guard method for "CreateOrUpdateTask"
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

        public HomeViewModel()
        {
            DueDate = DateTime.Now;
            NewTasks = new();
            InProgressTasks = new();
            CompletedTasks = new();
            Name = string.Empty;
            Description = string.Empty;
            SelectedStatus = Status.New;
            SubmitBtnContent = MessageStrings.Create;
        }


        public void CreateOrUpdateTask()
        {
            if (SubmitBtnContent.Equals(MessageStrings.Create, StringComparison.OrdinalIgnoreCase))
                CreateTask();
            else
                UpdateTask();
            ResetInputControls();
        }
        public void CreateTask()
        {
            switch (SelectedStatus)
            {
                case Status.New:
                    NewTasks.Add(new(Name, Description, SelectedStatus, SelectedPriority, DueDate, SelectedCategory, PercentageComplete));
                    break;
                case Status.InProgress:
                    InProgressTasks.Add(new(Name, Description, SelectedStatus, SelectedPriority, DueDate, SelectedCategory, PercentageComplete));
                    break;
                case Status.Completed:
                    CompletedTasks.Add(new(Name, Description, SelectedStatus, SelectedPriority, DueDate, SelectedCategory, PercentageComplete));
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
        }
        public void ResetInputControls()
        {
            Name = string.Empty;
            Description = string.Empty;
            SelectedPriority = Priority.Low;
            SelectedStatus = Status.New;
            SelectedCategory = Category.NewFeature;
            DueDate = DateTime.Now;
            SubmitBtnContent = MessageStrings.Create;
            PercentageComplete = 0;

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
                NewTasks.Add(task);
            }
        }
        public void DropOnInProgressTasks(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Task)) is Task task && task.Status != Status.InProgress)
            {
                RemoveTaskFromDragSource(task);
                task.Status = Status.InProgress;
                InProgressTasks.Add(task);
            }
        }
        public void DropOnCompletedTasks(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Task)) is Task task && task.Status != Status.Completed)
            {
                RemoveTaskFromDragSource(task);
                task.Status = Status.Completed;
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
        public async void DeleteById(Guid id, Status status)
        {
            if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(MessageStrings.ConfirmDeleteWinTitle, MessageStrings.ConfirmDeleteMsg, MessageDialogStyle.AffirmativeAndNegative))
            {
                try
                {
                    switch (status)
                    {
                        case Status.New:
                            NewTasks.Remove(NewTasks.FirstOrDefault(tsk => tsk.Id == id));
                            break;
                        case Status.InProgress:
                            InProgressTasks.Remove(InProgressTasks.FirstOrDefault(tsk => tsk.Id == id));
                            break;
                        case Status.Completed:
                            CompletedTasks.Remove(CompletedTasks.FirstOrDefault(tsk => tsk.Id == id));
                            break;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(MessageStrings.DeleteFailedMsg, MessageStrings.DeleteFailedWinTitle);
                }
            }

        }
        public void ShowSelectedTask()
        {
            if (SelectedTask != null)
            {
                SubmitBtnContent = MessageStrings.Update;
                Name = SelectedTask.Name;
                Description = SelectedTask.Description;
                SelectedStatus = SelectedTask.Status;
                SelectedPriority = SelectedTask.Priority;
                SelectedCategory = SelectedTask.Category;
                DueDate = SelectedTask.DueDate;
                PercentageComplete = SelectedTask.PercentageCompleted;
            }
            else
                ResetInputControls();
        }

    }
}
