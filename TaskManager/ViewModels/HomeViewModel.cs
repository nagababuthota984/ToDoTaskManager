using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManager.Common;
using TaskManager.Data;
using TaskManager.Models;
using static TaskManager.Models.Enums;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using TaskManager.Data;
using System.Diagnostics;
using System.Threading;

namespace TaskManager.ViewModels
{
    public class HomeViewModel : Conductor<Screen>, IHandle<Tuple<OperationType, Models.Task>>
    {
        #region Fields
        
        private Task _selectedTask;
        private BindableCollection<Task> _newTasks;
        private BindableCollection<Task> _inProgressTasks;
        private bool _isTaskFormEnabled;
        private BindableCollection<Task> _completedTasks;
        private string _searchKeyword;
        private bool _isListViewEnabled = false;
        private CreateTaskViewModel _createTaskView;
        private bool _isCardViewEnabled;
        private readonly ITaskRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private BindableCollection<Task> _tasks;
        private BindableCollection<Task> _filteredTasks;
        private int _currentPageNumber;
        private int _totalPagesCount;
        private int _itemsPerPage=10;
        #endregion

        #region Properties
        
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set { _selectedTask = value; NotifyOfPropertyChange(nameof(SelectedTask)); }
        }

        public BindableCollection<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                NotifyOfPropertyChange(nameof(Tasks));
            }
        }

        public BindableCollection<Task> FilteredTasks
        {
            get { return _filteredTasks; }
            set
            {
                _filteredTasks = value;
                NotifyOfPropertyChange(nameof(FilteredTasks));
            }
        }

        public BindableCollection<Task> NewTasks
        {
            get { return _newTasks; }
            set
            {
                _newTasks = value;
                NotifyOfPropertyChange(nameof(NewTasks));
            }

        }
       
        public BindableCollection<Task> InProgressTasks
        {
            get { return _inProgressTasks; }
            set
            {
                _inProgressTasks = value;
                NotifyOfPropertyChange(nameof(InProgressTasks));
            }
        }

        public BindableCollection<Task> CompletedTasks
        {
            get { return _completedTasks; }
            set
            {
                _completedTasks = value;
                NotifyOfPropertyChange(nameof(CompletedTasks));
            }
        }

        public int TotalPagesCount
        {
            get
            {
                return _totalPagesCount;
            }
            set
            {
                _totalPagesCount = value;
                NotifyOfPropertyChange(nameof(TotalPagesCount));
                NotifyOfPropertyChange(nameof(CanNavigateNext));
                NotifyOfPropertyChange(nameof(CanNavigatePrevious));
            }
        }

        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set
            {
                if (int.TryParse(value.ToString(), out int intValue))
                {
                    _itemsPerPage = intValue;
                    NotifyOfPropertyChange(nameof(ItemsPerPage));
                    NotifyOfPropertyChange(nameof(CanNavigateNext));
                    NotifyOfPropertyChange(nameof(CanNavigatePrevious));
                    InitializeListView();
                }
                else
                    _itemsPerPage = 0;
            }
        }

        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set
            {
                _currentPageNumber = value;
                NotifyOfPropertyChange(nameof(CurrentPageNumber));
                NotifyOfPropertyChange(nameof(CanNavigateNext));
                NotifyOfPropertyChange(nameof(CanNavigatePrevious));
            }
        }

        public bool CanSwitchToListView
        {
            get { return IsCardViewEnabled; }
        }

        public bool CanSwitchToCardView
        {
            get { return IsListViewEnabled; }
        }

        public string SearchKeyword
        {
            get { return _searchKeyword; }
            set { _searchKeyword = value; NotifyOfPropertyChange(nameof(SearchKeyword)); }
        }


        public bool IsListViewEnabled
        {
            get { return _isListViewEnabled; }
            set
            {
                _isListViewEnabled = value;
                NotifyOfPropertyChange(nameof(IsListViewEnabled));
                NotifyOfPropertyChange(nameof(CanSwitchToCardView));
                NotifyOfPropertyChange(nameof(CanSwitchToListView));
            }
        }

        public bool IsCardViewEnabled
        {
            get { return _isCardViewEnabled; }
            set
            {
                _isCardViewEnabled = value;
                NotifyOfPropertyChange(nameof(IsCardViewEnabled));
                NotifyOfPropertyChange(nameof(CanSwitchToCardView));
                NotifyOfPropertyChange(nameof(CanSwitchToListView));
            }
        }

        public bool CanNavigateNext
        {
            get { return CurrentPageNumber < TotalPagesCount; }
        }

        public bool CanNavigatePrevious
        {
            get { return CurrentPageNumber > 1; }
        }

        public CreateTaskViewModel CreateTaskView
        {
            get { return _createTaskView; }
            set { _createTaskView = value; NotifyOfPropertyChange(nameof(CreateTaskView)); }
        }


        public bool IsTaskFormEnabled
        {
            get { return _isTaskFormEnabled; }
            set { _isTaskFormEnabled = value; NotifyOfPropertyChange(nameof(IsTaskFormEnabled)); }
        }

        private bool _isGroupingEnabled;

        public bool IsGroupingEnabled
        {
            get { return _isGroupingEnabled; }
            set { _isGroupingEnabled = value; NotifyOfPropertyChange(nameof(IsGroupingEnabled)); }
        }



        #endregion

        public HomeViewModel(SqliteRepository sqliteRepository, SqlServerRepository sqlServerRepository, IEventAggregator eventAggregator,CreateTaskViewModel createTaskViewModel)
        {
            _repository = Application.Current.Properties[Constant.Database].ToString() == Constant.Sqlite ? sqliteRepository : sqlServerRepository;
            InitializeTaskLists();
            IsCardViewEnabled = true;
            CreateTaskView = createTaskViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        private void InitializeTaskLists()
        {
            NewTasks = new(_repository.GetAllTasks(Status.New));
            InProgressTasks = new(_repository.GetAllTasks(Status.InProgress));
            CompletedTasks = new(_repository.GetAllTasks(Status.Completed));
        }

        
        public void CreateTask(Task inputTask)
        {
            
            _repository.CreateTask(inputTask);
            AddTaskToUI(inputTask);

        }

        private void AddTaskToUI(Task task)
        {
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
            Tasks.Add(task);
            FilteredTasks.Add(task);
        }

        public void UpdateTask(Task inputTask)
        {
            Task SelectedTask = _repository.GetTaskById(inputTask.Id);
            RemoveTaskFromUIById(SelectedTask.Id, SelectedTask.Status);
            AddTaskToUI(inputTask);
            _repository.UpdateTask(inputTask);
        }

        private void RemoveTaskFromUIById(Guid id, Status status)
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
                FilteredTasks.Remove(FilteredTasks.FirstOrDefault(tsk => tsk.Id == id));
                Tasks.Remove(Tasks.FirstOrDefault(tsk => tsk.Id == id));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
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
            }
        }

        private void RemoveTaskFromUI(Task task)
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
        }

        public void DisplayTaskById(Guid id)
        {
            Task selectedTask = _repository.GetTaskById(id);
            IsTaskFormEnabled = true;
            _eventAggregator.PublishOnCurrentThreadAsync(Tuple.Create(OperationType.Display,selectedTask));
        }

        public void SwitchToListView()
        {
            IsListViewEnabled = true;
            IsCardViewEnabled = false;
            if (Tasks == null)
                InitializeListView();
        }

        private void InitializeListView()
        {
            Tasks = new(_repository.GetAllTasks());
            FilteredTasks = new(Tasks.Take(ItemsPerPage));
            CurrentPageNumber = 1;
            TotalPagesCount = (Tasks.Count + ItemsPerPage - 1) / ItemsPerPage;
        }

        public void SwitchToCardView()
        {
            IsListViewEnabled = false;
            IsCardViewEnabled = true;
        }

        public void SearchTasks()
        {
            if (!string.IsNullOrWhiteSpace(SearchKeyword) && SearchKeyword.Length > 2)
                FilteredTasks = FilteredTasks.Count > 0 ? new(FilteredTasks.Where(tsk => tsk.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))) : new();
            else
                LoadCurrentPage();
        }

        public void NavigateNext()
        {

            FilteredTasks = new(Tasks.Skip(CurrentPageNumber * ItemsPerPage).Take(ItemsPerPage));
            CurrentPageNumber += 1;
        }

        public void NavigatePrevious()
        {
            FilteredTasks = new(Tasks.Skip((CurrentPageNumber - 2) * ItemsPerPage).Take(ItemsPerPage));
            CurrentPageNumber -= 1;
        }

        public void LoadCurrentPage()
        {
            FilteredTasks = new(Tasks.Skip((CurrentPageNumber - 1) * ItemsPerPage).Take(ItemsPerPage));
        }

        public void CloseTaskForm()
        {
            IsTaskFormEnabled = false;
        }

        public System.Threading.Tasks.Task HandleAsync(Tuple<OperationType, Task> message, CancellationToken cancellationToken)
        {
            if (message.Item1 == OperationType.Create)
                CreateTask(new(message.Item2));
            else if (message.Item1 == OperationType.Update)
                UpdateTask(new(message.Item2));
            IsTaskFormEnabled = false;
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}