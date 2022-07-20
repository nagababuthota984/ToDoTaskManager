using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Common;
using static TaskManager.Models.Enums;

namespace TaskManager.ViewModels
{
    public class CreateTaskViewModel : Screen, IHandle<Tuple<OperationType, Models.Task>>
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
                _eventAggregator.PublishOnUIThreadAsync(Tuple.Create(OperationType.Create, InputTask));
            }
            
        }

        public void UpdateTask()
        {
            if (InputTask != null && !string.IsNullOrWhiteSpace(InputTask.Name))
            {
                _eventAggregator.PublishOnUIThreadAsync(Tuple.Create(OperationType.Update, InputTask));
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

        public Task HandleAsync(Tuple<OperationType, Models.Task> message, CancellationToken cancellationToken)
        {
            if (message.Item1 == OperationType.Display)
            {
                UserRole = UserRole.Edit;
                InputTask = new(message.Item2);
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
