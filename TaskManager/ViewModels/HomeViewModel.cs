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
        private ObservableCollection<Task> _completedFilteredTasks;
        private Priority _selectedPriority;
        private TaskViewMode _selectedTaskView;
        private Status _selectedStatus;
        private string _submitBtnContent;
        private float _percentageComplete;
        private string _searchKeyword;
        private Category _selectedCategory;
        private ObservableCollection<Task> _newFilteredTasks;
        private ObservableCollection<Task> _inProgressFilteredTasks;
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
        public ObservableCollection<Task> FilteredNewTasks
        {
            get { return _newFilteredTasks; }
            set
            {
                _newFilteredTasks = value;
                NotifyOfPropertyChange(nameof(FilteredNewTasks));
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
        public ObservableCollection<Task> FilteredInProgressTasks
        {
            get { return _inProgressFilteredTasks; }
            set
            {
                _inProgressFilteredTasks = value;
                NotifyOfPropertyChange(nameof(FilteredInProgressTasks));
            }
        }
        public ObservableCollection<Task> CompletedTasks
        {
            get { return _completedFilteredTasks; }
            set
            {
                _completedFilteredTasks = value;
                NotifyOfPropertyChange(nameof(CompletedTasks));
            }
        }
        public ObservableCollection<Task> FilteredCompletedTasks
        {
            get { return _completedFilteredTasks; }
            set
            {
                _completedFilteredTasks = value;
                NotifyOfPropertyChange(nameof(FilteredCompletedTasks));
            }
        }
        public TaskViewMode SelectedTaskView
        {
            get { return _selectedTaskView; }
            set
            {
                _selectedTaskView = value;
                NotifyOfPropertyChange(nameof(SelectedTaskView));
                NotifyOfPropertyChange(nameof(CanSwitchToListView));
                NotifyOfPropertyChange(nameof(CanSwitchToCardView));
            }
        }
        public bool CanCreateOrUpdateTask
        {
            get { return !string.IsNullOrWhiteSpace(Name); }
        }
        public bool CanSwitchToListView
        {
            get { return SelectedTaskView == TaskViewMode.Card; }
        }
        public bool CanSwitchToCardView
        {
            get { return SelectedTaskView == TaskViewMode.List; }
        }
        public string SubmitBtnContent
        {
            get { return _submitBtnContent; }
            set { _submitBtnContent = value; NotifyOfPropertyChange(nameof(SubmitBtnContent)); }
        }
        public string SearchKeyword
        {
            get { return _searchKeyword; }
            set { _searchKeyword = value; NotifyOfPropertyChange(nameof(SearchKeyword));}
        }
        public UserRole UserRole
        {
            get; set;
        }

        #endregion

        public HomeViewModel(SqliteRepository sqliteRepository, SqlServerRepository sqlServerRepository)
        {
            _repository = Application.Current.Properties[Constant.Database].ToString() == Constant.Sqlite ? sqliteRepository : sqlServerRepository;
            InitializeTaskLists();
            Name = string.Empty;
            Description = string.Empty;
            DueDate = DateTime.Now;
            SelectedStatus = Status.New;
            SelectedTaskView = TaskViewMode.Card;
            UserRole = UserRole.Create;
            SubmitBtnContent = Constant.Create;
           
        }

        private void InitializeTaskLists()
        {
            NewTasks = new(_repository.GetAllTasks(Status.New));
            InProgressTasks = new(_repository.GetAllTasks(Status.InProgress));
            CompletedTasks = new(_repository.GetAllTasks(Status.Completed));
            FilteredNewTasks = new(NewTasks);
            FilteredInProgressTasks = new(InProgressTasks);
            FilteredCompletedTasks = new(CompletedTasks);
        }

        public void CreateOrUpdateTask()
        {
            if (UserRole == UserRole.Create)
                CreateTask();
            else
                UpdateTask();
            ResetInputControls();
        }

        public void CreateTask()
        {
            Task task = new(Name)
            {
                Description = Description,
                Status = SelectedStatus,
                Priority = SelectedPriority,
                DueDate = DueDate,
                Category = SelectedCategory,
                PercentageCompleted = PercentageComplete
            };
            _repository.CreateTask(task);
            AddTaskToUI(task);

        }

        private void AddTaskToUI(Task task)
        {
            switch (task.Status)
            {
                case Status.New:
                    NewTasks.Add(task);
                    FilteredNewTasks.Add(task);
                    break;
                case Status.InProgress:
                    InProgressTasks.Add(task);
                    FilteredInProgressTasks.Add(task);
                    break;
                case Status.Completed:
                    CompletedTasks.Add(task);
                    FilteredCompletedTasks.Add(task);
                    break;
            }
        }

        public void UpdateTask()
        {
            Task task = new()
            {
                Id = SelectedTask.Id,
                Name = Name,
                Description = Description,
                Status = SelectedStatus,
                Priority = SelectedPriority,
                Category = SelectedCategory,
                PercentageCompleted = PercentageComplete,
                DueDate = DueDate
            };
            RemoveTaskFromUIById(SelectedTask.Id,SelectedTask.Status);
            AddTaskToUI(task);
            _repository.UpdateTask(task);
        }

        private void RemoveTaskFromUIById(Guid id, Status status)
        {
            try
            {
                switch (status)
                {
                    case Status.New:
                        NewTasks.Remove(NewTasks.FirstOrDefault(tsk => tsk.Id == id));
                        FilteredNewTasks.Remove(FilteredNewTasks.FirstOrDefault(tsk => tsk.Id == id));
                        break;
                    case Status.InProgress:
                        InProgressTasks.Remove(InProgressTasks.FirstOrDefault(tsk => tsk.Id == id));
                        FilteredInProgressTasks.Remove(FilteredInProgressTasks.FirstOrDefault(tsk => tsk.Id == id));
                        break;
                    case Status.Completed:
                        CompletedTasks.Remove(CompletedTasks.FirstOrDefault(tsk => tsk.Id == id));
                        FilteredCompletedTasks.Remove(FilteredCompletedTasks.FirstOrDefault(tsk => tsk.Id == id));
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
        }

        public void ResetInputControls()
        {
            Description = Name = string.Empty;
            SelectedPriority = Priority.Low;
            SelectedStatus = Status.New;
            SelectedCategory = Category.NewFeature;
            DueDate = DateTime.Now;
            UserRole = UserRole.Create;
            SubmitBtnContent = Constant.Create;
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
                RemoveTaskFromUI(task);
                task.Status = Status.New;
                _repository.UpdateTask(task);
                NewTasks.Add(task);
                FilteredNewTasks.Add(task);
            }
        }

        public void DropOnInProgressTasks(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Task)) is Task task && task.Status != Status.InProgress)
            {
                RemoveTaskFromUI(task);
                task.Status = Status.InProgress;
                _repository.UpdateTask(task);
                InProgressTasks.Add(task);
                FilteredInProgressTasks.Add(task);
            }
        }

        public async void DropOnCompletedTasks(DragEventArgs e)
        {
            if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(Constant.TaskCompletedWinTitle, Constant.TaskCompletedMsg, MessageDialogStyle.AffirmativeAndNegative) && e.Data.GetData(typeof(Task)) is Task task && task.Status != Status.Completed)
            {
                RemoveTaskFromUI(task);
                task.Status = Status.Completed;
                _repository.UpdateTask(task);
                CompletedTasks.Add(task);
                FilteredCompletedTasks.Add(task);
            }
        }

        private void RemoveTaskFromUI(Task task)
        {
            switch (task.Status)
            {
                case Status.New:
                    NewTasks.Remove(task);
                    FilteredNewTasks.Remove(task);
                    break;
                case Status.InProgress:
                    InProgressTasks.Remove(task);
                    FilteredInProgressTasks.Remove(task);
                    break;
                case Status.Completed:
                    CompletedTasks.Remove(task);
                    FilteredCompletedTasks.Remove(task);
                    break;
            }
        }

        public async void DeleteById(Guid id, Status status)
        {
            if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(Constant.ConfirmDeleteWinTitle, Constant.ConfirmDeleteMsg, MessageDialogStyle.AffirmativeAndNegative))
            {
                try
                {
                    _repository.DeleteTaskById(id);
                    RemoveTaskFromUIById(id, status);
                }
                catch (Exception)
                {
                    MessageBox.Show(Constant.DeleteFailedMsg, Constant.DeleteFailedWinTitle);
                }
            }
            ResetInputControls();
        }
        public void DisplayTaskById(Guid id)
        {
            SelectedTask = _repository.GetTaskById(id);
            if (SelectedTask != null)
            {
                UserRole = UserRole.Edit;
                SubmitBtnContent = Constant.Update;
                Name = SelectedTask.Name;
                Description = SelectedTask.Description;
                SelectedStatus = SelectedTask.Status;
                SelectedPriority = SelectedTask.Priority;
                SelectedCategory = SelectedTask.Category;
                DueDate = SelectedTask.DueDate;
                PercentageComplete = SelectedTask.PercentageCompleted;
            }
        }

        public void SwitchToListView()
        {
            SelectedTaskView = TaskViewMode.List;
        }
        public void SwitchToCardView()
        {
            SelectedTaskView = TaskViewMode.Card;
        }
        public void SearchTasks()
        {
           if(!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                FilteredNewTasks = NewTasks.Count>0 ? new(NewTasks.Where(tsk => tsk.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))): new();
                FilteredInProgressTasks = InProgressTasks.Count>0 ?new(InProgressTasks.Where(tsk => tsk.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))): new();
                FilteredCompletedTasks = CompletedTasks.Count>0 ? new(CompletedTasks.Where(tsk => tsk.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))) : new();
            }
           else
            {
                FilteredNewTasks = new(NewTasks);
                FilteredInProgressTasks = new(InProgressTasks);
                FilteredCompletedTasks = new(CompletedTasks);
            }    
            
        }
    }
}