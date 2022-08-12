﻿using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data;
using TaskManager.Helpers;
using TaskManager.Models;
using static TaskManager.Models.Enums;

namespace TaskManager.ViewModels
{
    public class ListViewModel : Screen, IHandle<TaskEventMessage>
    {
        #region Fields
        private bool _isGroupingEnabled;
        private int _currentPageNumber;
        private ITaskRepository _repository;
        private readonly SimpleContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private string _searchKeyword;
        private int _totalPagesCount;
        private int _itemsPerPage = 10;
        private CreateTaskViewModel _createTaskView;
        private bool _isTaskFormEnabled;
        private BindableCollection<Task> _tasks;
        private static int _activeListViewModelId;
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

        public static int ActiveListViewModelId
        {
            get
            {
                return _activeListViewModelId;
            }
            set
            {
                _activeListViewModelId = value;
            }
        }
        #endregion

        public ListViewModel(SimpleContainer container, IEventAggregator eventAggregator, CreateTaskViewModel createTaskViewModel)
        {
            _container = container;
            CreateTaskView = createTaskViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            _repository = DbContextFactory.TaskRepository;
            InitializeTaskLists();
        }

        private void InitializeTaskLists()
        {
            Tasks = new(_repository.GetTasks());
            //Tasks = new(DataGenerator.CreateTasks(100));
            FilteredTasks = new(Tasks.Take(ItemsPerPage));
            TotalPagesCount = (Tasks.Count + ItemsPerPage - 1) / ItemsPerPage;
            CurrentPageNumber = 1;
        }

        public async System.Threading.Tasks.Task SearchTasks()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                if (!string.IsNullOrWhiteSpace(SearchKeyword))
                    FilteredTasks = Tasks.Count > 0 ? new(Tasks.Where(tsk => tsk.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))) : new();
                else
                    LoadCurrentPage();
            });
        }

        public async System.Threading.Tasks.Task DeleteTaskById(Guid id)
        {
            if (await DialogHelper.ShowMessageDialog(Constant.ConfirmDeleteWinTitle, Constant.ConfirmDeleteMsg, MessageDialogStyle.AffirmativeAndNegative))
            {
                try
                {
                    Task task = Tasks.FirstOrDefault(tsk => tsk.Id == id);
                    _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() { Sender = this, OperationType = OperationType.Delete, Task = task });
                    RemoveTaskFromUI(id);
                    _repository.DeleteTask(id);
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
            try
            {
                FilteredTasks.Remove(Tasks.FirstOrDefault(tsk => tsk.Id == id));
                Tasks.Remove(Tasks.FirstOrDefault(tsk => tsk.Id == id));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void DisplayTaskById(Guid id)
        {
            Task selectedTask = _repository.GetTask(id);
            if (selectedTask != null)
            {
                _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() { Sender = this, Task = selectedTask, OperationType = OperationType.Display });
                IsTaskFormEnabled = true;
            }
        }

        private void UpdateTask(Task task)
        {
            RemoveTaskFromUI(task.Id);
            AddTaskToUI(task);
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
            if (message != null && message.Sender.GetHashCode() != this.GetHashCode() && this.GetHashCode() == ActiveListViewModelId)
            {
                switch (message.OperationType)
                {
                    case OperationType.Create:
                        AddTaskToUI(message.Task);
                        break;
                    case OperationType.Update:
                        UpdateTask(message.Task);
                        break;
                    case OperationType.Delete:
                        RemoveTaskFromUI(message.Task.Id);
                        break;
                }
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
