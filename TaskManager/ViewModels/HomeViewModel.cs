using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TaskManager.Common;
using TaskManager.Data;
using TaskManager.Models;
using static TaskManager.Models.Enums;

namespace TaskManager.ViewModels
{
    public class HomeViewModel : Conductor<Screen>, IHandle<TaskEventMessage>
    {
        #region Fields

        private bool _isListViewEnabled;
        private bool _isCardViewEnabled;
        private readonly ITaskRepository _repository;
        private readonly SimpleContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private CreateTaskViewModel _createTaskView;
        private ListViewModel _listViewModel;
        private BindableCollection<Task> _newTasks;
        private BindableCollection<Task> _inProgressTasks;
        private BindableCollection<Task> _completedTasks;

        #endregion

        #region Properties

        public BindableCollection<Task> NewTasks
        {
            get
            {
                return _newTasks;
            }
            set
            {
                _newTasks = value;
                NotifyOfPropertyChange(nameof(NewTasks));
            }

        }

        public BindableCollection<Task> InProgressTasks
        {
            get
            {
                return _inProgressTasks;
            }
            set
            {
                _inProgressTasks = value;
                NotifyOfPropertyChange(nameof(InProgressTasks));
            }
        }

        public BindableCollection<Task> CompletedTasks
        {
            get
            {
                return _completedTasks;
            }
            set
            {
                _completedTasks = value;
                NotifyOfPropertyChange(nameof(CompletedTasks));
            }
        }

        public bool CanSwitchToListView
        {
            get
            {
                return IsCardViewEnabled;
            }
        }

        public bool CanSwitchToCardView
        {
            get
            {
                return IsListViewEnabled;
            }
        }

        public bool IsListViewEnabled
        {
            get
            {
                return _isListViewEnabled;
            }
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
            get
            {
                return _isCardViewEnabled;
            }
            set
            {
                _isCardViewEnabled = value;
                NotifyOfPropertyChange(nameof(IsCardViewEnabled));
                NotifyOfPropertyChange(nameof(CanSwitchToCardView));
                NotifyOfPropertyChange(nameof(CanSwitchToListView));
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

        public ListViewModel ListViewModel
        {
            get
            {
                return _listViewModel;
            }
            set
            {
                _listViewModel = value;
                NotifyOfPropertyChange(nameof(ListView));
            }
        }

        #endregion

        #region Constructors

        public HomeViewModel(IEventAggregator eventAggregator, CreateTaskViewModel createTaskViewModel, ListViewModel listViewModel, SimpleContainer container)
        {
            _container = container;
            _repository = Application.Current.Properties[Constant.Database] == Constant.Sqlite ? _container.GetInstance<ITaskRepository>(nameof(SqliteRepository)) : _container.GetInstance<ITaskRepository>(nameof(SqlServerRepository));
            InitializeTaskLists();
            CreateTaskView = createTaskViewModel;
            ListViewModel = listViewModel;
            IsCardViewEnabled = true;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            InitializeNotificationTimer();
        }

        #endregion

        #region Methods

        private void InitializeTaskLists()
        {
            var tasks = _repository.GetTasks();
            NewTasks = new(tasks.Where(tsk => tsk.Status == Status.New));
            InProgressTasks = new(tasks.Where(tsk => tsk.Status == Status.InProgress));
            CompletedTasks = new(tasks.Where(tsk => tsk.Status == Status.Completed));
        }

        public void CreateTask(Task inputTask)
        {
            _repository.CreateTask(inputTask);
            AddTaskToUI(inputTask);
        }

        public void UpdateTask(Task inputTask)
        {
            Task oldTask = _repository.GetTaskById(inputTask.Id);
            if (oldTask != null)
            {
                oldTask = new(inputTask);
                _repository.UpdateTask(inputTask);
            }
            else
                MessageBox.Show(Constant.UpdateFailed, Constant.ErrorOccured);
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
        }

        public void DisplayTaskById(Guid id)
        {
            Task selectedTask = _repository.GetTaskById(id);
            if (selectedTask != null)
            {
                _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() { Sender = this, Task = selectedTask, OperationType = OperationType.Display });
            }
        }

        /// <summary>
        /// IsForcedDelete is true when the delete is raised through an event.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="isForcedDelete"></param>
        public async void DeleteById(Guid id, Status status, bool isForcedDelete = false)
        {
            if (!isForcedDelete && !await Constant.ShowMessageDialog(Constant.ConfirmDeleteWinTitle, Constant.ConfirmDeleteMsg, MessageDialogStyle.AffirmativeAndNegative))
                return;
            else
            {
                try
                {
                    _repository.DeleteTaskById(id);
                    RemoveTaskFromUI(id, status, true);
                    await _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() { Sender = this, OperationType = OperationType.Delete, Task = new() { Id = id } });
                }
                catch (Exception)
                {
                    MessageBox.Show(Constant.DeleteFailedMsg, Constant.DeleteFailedWinTitle);
                }
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

        /// <summary>
        /// isForcedDelete is true when the delete action is raised through events
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="isForcedDelete"></param>
        private void RemoveTaskFromUI(Guid id, Status status, bool isForcedDelete = false)
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
                if (!isForcedDelete)
                    _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() { Sender = this, OperationType = OperationType.Delete, Task = new Task() { Id = id, Status = status } });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
        }

        public async void SwitchToListView()
        {
            IsListViewEnabled = true;
            IsCardViewEnabled = false;
        }

        public void SwitchToCardView()
        {
            IsListViewEnabled = false;
            IsCardViewEnabled = true;
            ActivateItemAsync(_container.GetInstance<ListViewModel>());
        }

        #endregion

        #region Drag and Drop helpers

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
        //TODO: remove showmessageasync
        public async void DropOnCompletedTasks(DragEventArgs e)
        {
            if (await Constant.ShowMessageDialog(Constant.TaskCompletedWinTitle, Constant.TaskCompletedMsg, MessageDialogStyle.AffirmativeAndNegative) && e.Data.GetData(typeof(Task)) is Task task && task.Status != Status.Completed)
            {
                RemoveTaskFromUI(task);
                task.Status = Status.Completed;
                _repository.UpdateTask(task);
                CompletedTasks.Add(task);
            }
        }

        #endregion

        #region Notification Helper methods

        private void InitializeNotificationTimer()
        {
            DispatcherTimer timer = new();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromHours(1);
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            List<Task> tasks = NewTasks.Where((tsk => tsk.DueDate <= DateTime.Now.AddHours(1) && tsk.DueDate >= DateTime.Now.Subtract(TimeSpan.FromHours(1)))).ToList();
            tasks.AddRange(InProgressTasks.Where((tsk => tsk.DueDate <= DateTime.Now.AddHours(1) && tsk.DueDate >= DateTime.Now.Subtract(TimeSpan.FromHours(1)))));
            string message;
            switch (tasks.Count)
            {
                case 0: break;
                case 1:
                    message = $"{tasks[0].Name} is nearing due time";
                    RaiseToastNotification(Constant.TaskDue, message);
                    break;
                default:
                    message = $"You have {tasks.Count} tasks nearing due time";
                    RaiseToastNotification(Constant.TaskDue, message);
                    break;
            }
        }

        private void RaiseToastNotification(string title, string message, ToastScenario toastScenario = ToastScenario.Reminder, ToastDuration toastDuration = ToastDuration.Long)
        {
            new ToastContentBuilder()
                .AddAppLogoOverride(Constant.IconPath)
                .SetToastScenario(toastScenario)
                .SetToastDuration(toastDuration)
                .AddText(title)
                .AddText(message)
                .Show();
        }

        #endregion

        #region EventHandlers

        public System.Threading.Tasks.Task HandleAsync(TaskEventMessage message, CancellationToken cancellationToken)
        {

            if (message.Sender.GetHashCode() != this.GetHashCode())
            {
                if (message.OperationType == OperationType.Display)
                    return System.Threading.Tasks.Task.CompletedTask;
                else if (message.OperationType == OperationType.Create)
                    CreateTask(new(message.Task));
                else if (message.OperationType == OperationType.Update)
                    UpdateTask(new(message.Task));
                else if (message.OperationType == OperationType.Delete)
                    DeleteById(message.Task.Id, message.Task.Status, true);
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }

        #endregion

        #region Overriden Methods

        protected override System.Threading.Tasks.Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            return base.OnDeactivateAsync(close, cancellationToken);
        }


        #endregion


    }
}