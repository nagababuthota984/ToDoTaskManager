using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data;
using TaskManager.Models;
using static TaskManager.Models.Enums;

namespace TaskManager.ViewModels
{
    public class ListViewModel : Screen, IHandle<TaskEventMessage>
    {
        #region Fields
        private bool _isGroupingEnabled;
        private int _currentPageNumber;
        private readonly ITaskRepository _repository;
        private readonly SimpleContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private string _searchKeyword;
        private int _totalPagesCount;
        private int _itemsPerPage = 10;
        private CreateTaskViewModel _createTaskView;
        private bool _isTaskFormEnabled;
        private BindableCollection<Task> _tasks;
        private BindableCollection<Task> _filteredTasks;
        #endregion

        #region Properties
        public BindableCollection<Task> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
                NotifyOfPropertyChange(nameof(Tasks));
            }
        }

        public BindableCollection<Task> FilteredTasks
        {
            get
            {
                return _filteredTasks;
            }
            set
            {
                _filteredTasks = value;
                NotifyOfPropertyChange(nameof(FilteredTasks));
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
                    InitializeTaskLists();
                }
                else
                {
                    _itemsPerPage = 10;
                }
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

        public bool CanNavigateNext
        {
            get
            {
                return CurrentPageNumber < TotalPagesCount;
            }
        }

        public string SearchKeyword
        {
            get
            {
                return _searchKeyword;
            }
            set
            {
                _searchKeyword = value;
                NotifyOfPropertyChange(nameof(SearchKeyword));
            }
        }

        public bool CanNavigatePrevious
        {
            get
            {
                return CurrentPageNumber > 1;
            }
        }

        public CreateTaskViewModel CreateTaskView
        {
            get
            {
                return _createTaskView;
            }
            set
            {
                _createTaskView = value;
                NotifyOfPropertyChange(nameof(CreateTaskView));
            }
        }

        public bool IsTaskFormEnabled
        {
            get
            {
                return _isTaskFormEnabled;
            }
            set
            {
                _isTaskFormEnabled = value;
                NotifyOfPropertyChange(nameof(IsTaskFormEnabled));
            }
        }

        public bool IsGroupingEnabled
        {
            get
            {
                return _isGroupingEnabled;
            }
            set
            {
                _isGroupingEnabled = value;
                NotifyOfPropertyChange(nameof(IsGroupingEnabled));
            }
        }
        #endregion

        public ListViewModel(SimpleContainer container, IEventAggregator eventAggregator, CreateTaskViewModel createTaskViewModel)
        {
            _container = container;
            CreateTaskView = createTaskViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            _repository = Application.Current.Properties[Constant.Database] == Constant.Sqlite ? _container.GetInstance<ITaskRepository>(nameof(SqliteRepository)) : _container.GetInstance<ITaskRepository>(nameof(SqlServerRepository));
            InitializeTaskLists();
        }

        private void InitializeTaskLists()
        {
            Tasks = new(_repository.GetTasks());
            FilteredTasks = new(Tasks.Take(ItemsPerPage));
            TotalPagesCount = (Tasks.Count + ItemsPerPage - 1) / ItemsPerPage;
            CurrentPageNumber = 1;
        }

        public void SearchTasks()
        {
            if (!string.IsNullOrWhiteSpace(SearchKeyword) && SearchKeyword.Length > 2)
            {
                FilteredTasks = FilteredTasks.Count > 0 ? new(FilteredTasks.Where(tsk => tsk.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))) : new();
            }
            else
            {
                LoadCurrentPage();
            }
        }

        public async void DeleteTaskById(Guid id)
        {
            if (await Constant.ShowMessageDialog(Constant.ConfirmDeleteWinTitle, Constant.ConfirmDeleteMsg, MessageDialogStyle.AffirmativeAndNegative))
            {
                try
                {
                    Task task = Tasks.FirstOrDefault(tsk => tsk.Id == id);
                    _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() { Sender = this, OperationType = OperationType.Delete, Task = task });
                    RemoveTaskFromUI(id);
                }
                catch (Exception)
                {
                    MessageBox.Show(Constant.DeleteFailedMsg, Constant.DeleteFailedWinTitle);
                }

            }
        }

        public void AddTaskToUI(Task task)
        {
            Tasks.Add(task);
            FilteredTasks.Add(task);
        }

        public void RemoveTaskFromUI(Guid id)
        {
            FilteredTasks.Remove(Tasks.FirstOrDefault(tsk => tsk.Id == id));
            Tasks.Remove(Tasks.FirstOrDefault(tsk => tsk.Id == id));
        }

        public void DisplayTaskById(Guid id)
        {
            Task selectedTask = _repository.GetTaskById(id);
            if (selectedTask != null)
            {
                _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() { Sender = this, Task = selectedTask, OperationType = OperationType.Display });
                IsTaskFormEnabled = true;
            }
        }

        private void UpdateTask(Task task)
        {
            Task oldTask = Tasks.FirstOrDefault(tsk => tsk.Id == task.Id);
            oldTask = new(task);
            Task oldFilteredTask = FilteredTasks.FirstOrDefault(tsk => tsk.Id == task.Id);
            oldFilteredTask = new(task);
            _repository.UpdateTask(task);
        }

        public void CloseTaskForm()
        {
            IsTaskFormEnabled = false;
        }

        #region Pagination Helper Methods

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

        

        #endregion

        #region EventHandlers
        public System.Threading.Tasks.Task HandleAsync(TaskEventMessage message, CancellationToken cancellationToken)
        {
            if (message.Sender.GetHashCode() != this.GetHashCode())
            {
                if (message != null && message.OperationType == OperationType.Delete)
                    RemoveTaskFromUI(message.Task.Id);
                else if (message != null && message.OperationType == OperationType.Create)
                    AddTaskToUI(message.Task);
                else if (message != null && message.OperationType == OperationType.Update)
                    UpdateTask(message.Task);
                CloseTaskForm();
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
        #endregion

        #region overriden methods
        protected override System.Threading.Tasks.Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            return base.OnDeactivateAsync(close, cancellationToken);
        }
        #endregion
    }
}
