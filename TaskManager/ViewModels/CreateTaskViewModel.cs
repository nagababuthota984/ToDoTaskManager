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
    public class CreateTaskViewModel : Screen,IHandle<Tuple<OperationType,Models.Task>>
    {
        private IEventAggregator _eventAggregator;
        private Models.Task _task;
        private string _submitBtnContent;

        private UserRole _userRole;

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

        //public bool CanCreateOrUpdateTask
        //{
        //    get { return !string.IsNullOrWhiteSpace(Task.Name); }
        //}

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
            _eventAggregator.PublishOnCurrentThreadAsync(Tuple.Create(OperationType.Create, InputTask));
            ResetInputControls();
        }
        public void UpdateTask()
        {
            _eventAggregator.PublishOnCurrentThreadAsync(Tuple.Create(OperationType.Create, InputTask));
            ResetInputControls();
        }
        public void ResetInputControls()
        {
            InputTask = new();
            UserRole = UserRole.Create;
            SubmitBtnContent = Constant.Create;
        }

        public Task HandleAsync(Tuple<OperationType, Models.Task> message, CancellationToken cancellationToken)
        {
            if (message.Item1 == OperationType.Display)
            {
                UserRole = UserRole.Edit;
                InputTask = new(message.Item2);
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
        public void Cancel()
        {
            ResetInputControls();
        }
        public override object GetView(object context = null)
        {
            return null;
        }
    }
}
