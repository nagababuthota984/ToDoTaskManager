using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Common;
using TaskManager.Models;
using static TaskManager.Models.Enums;

namespace TaskManager.ViewModels
{
    public class CreateTaskViewModel : Screen, IHandle<TaskEventMessage>
    {
        #region Fields
        private IEventAggregator _eventAggregator;
        private Models.Task _task;
        private string _submitBtnContent;
        private UserRole _userRole;
        private string _name;
        #endregion

        #region Properties

        public UserRole UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                if (value == UserRole.Create) SubmitBtnContent = Constant.Create;
                else SubmitBtnContent = Constant.Update;
            }
        }

        public Models.Task InputTask
        {
            get { return _task; }
            set { _task = value; NotifyOfPropertyChange(nameof(InputTask)); }
        }

        public IEnumerable<Status> StatusOptions
        {
            get
            {
                return Enum.GetValues(typeof(Status)).Cast<Status>();
            }
        }

        public string SubmitBtnContent
        {
            get { return _submitBtnContent; }
            set { _submitBtnContent = value; NotifyOfPropertyChange(nameof(SubmitBtnContent)); }
        }

        #endregion

        #region Constructors

        public CreateTaskViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            UserRole = UserRole.Create;
            InputTask = new()
            {
                Name = string.Empty,
                Description = string.Empty,
                Status = Status.New,
                Priority = Priority.Low,
                DueDate = DateTime.Now,
                Category = Category.NewFeature

            };
            _eventAggregator.SubscribeOnUIThread(this);

        }

        #endregion

        #region Methods

        public void CreateOrUpdateTask()
        {
            if (UserRole == UserRole.Create)
            {
                CreateTask();
            }
            else
            {
                UpdateTask();
            }

            ResetInputControls();
        }

        public void CreateTask()
        {
            if (InputTask != null && !string.IsNullOrWhiteSpace(InputTask.Name))
            {
                _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() {Sender = this, Task = InputTask, OperationType = OperationType.Create });
            }
            
        }

        public void UpdateTask()
        {
            if (InputTask != null && !string.IsNullOrWhiteSpace(InputTask.Name))
            {
                _eventAggregator.PublishOnUIThreadAsync(new TaskEventMessage() {Sender = this, Task=InputTask,OperationType=OperationType.Update});
            }
        }

        public void ResetInputControls()
        {
            InputTask = new();
            UserRole = UserRole.Create;
            SubmitBtnContent = Constant.Create;
        }

        public void Cancel()
        {
            ResetInputControls();
        }

        #endregion

        #region EventHandlers

        public System.Threading.Tasks.Task HandleAsync(TaskEventMessage message, CancellationToken cancellationToken)
        {
            if (message.Sender !=this && message.OperationType == OperationType.Display)
            {
                UserRole = UserRole.Edit;
                InputTask = new(message.Task);
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }

        #endregion

        #region Overrides

        public override object GetView(object context = null)
        {
            return null;
        }

        #endregion
    }
}
