using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManager.Models;
using static TaskManager.Models.Enums;

namespace TaskManager.ViewModels
{
    public class HomeViewModel : Screen
    {
        #region Fields
        private DateTime _dueData;
        private ObservableCollection<Task> _newTasks;
        private ObservableCollection<Task> _inProgressTasks;
        private ObservableCollection<Task> _completedTasks;
        private Priority _selectedPriority;
        private Status _selectedStatus;
        private List<string> _statusOptions;
        #endregion
        #region Properties
        public DateTime DueDate
        {
            get
            {
                return _dueData;
            }
            set
            {
                _dueData = value;
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
                NotifyOfPropertyChange(nameof(SelectedStatus));
            }
        }
        public List<string> StatusOptions
        {
            get
            {
                return _statusOptions;
            }
            set
            {
                _statusOptions = value; NotifyOfPropertyChange(nameof(StatusOptions));
            }
        }
        

        #endregion

        public HomeViewModel()
        {
            DueDate = DateTime.Now;
            StatusOptions = Enum.GetNames<Status>().ToList();
            NewTasks = new();
            InProgressTasks = new();
            CompletedTasks = new();
        }
        public void Create(string name,string description)
        {
            if (SelectedStatus == Status.New)
                NewTasks.Add(new(name, description, SelectedStatus, SelectedPriority, DueDate));
            else if (SelectedStatus == Status.InProgress)
                InProgressTasks.Add(new(name, description, SelectedStatus, SelectedPriority, DueDate));
            else
                CompletedTasks.Add(new(name, description, SelectedStatus, SelectedPriority, DueDate));

        }
        public void Cancel()
        {
            //reset inputs
        }
        public void MouseMoveHandler(MouseEventArgs e)
        {
            if ( e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.Source is ListBox lbox)
                    DragDrop.DoDragDrop(lbox, lbox.SelectedItem, DragDropEffects.Move);
            }
        }
        public void DropHandler(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Task)) is Task task && e.Source is ListBox lb)
                AddTaskToTarget(task, lb.Name);
        }
        private void AddTaskToTarget(Task task, string targetControlName)
        {
            if (task.Status!=Status.New && targetControlName.Equals("newlist", StringComparison.OrdinalIgnoreCase))
            {
                RemoveTaskFromSource(task);
                task.Status = Status.New;
                NewTasks.Add(task);
            }
            else if (targetControlName.Equals("inprogresslist", StringComparison.OrdinalIgnoreCase))
            {
                RemoveTaskFromSource(task);
                task.Status = Status.InProgress;
                InProgressTasks.Add(task);
            }
            else if(task.Status!=Status.Completed && targetControlName.Equals("completedlist",StringComparison.OrdinalIgnoreCase))
            {
                RemoveTaskFromSource(task);
                task.Status = Status.Completed;
                CompletedTasks.Add(task);
            }
        }

        private void RemoveTaskFromSource(Task task)
        {
            if (task.Status == Status.New)
                NewTasks.Remove(task);
            else if (task.Status == Status.InProgress)
                InProgressTasks.Remove(task);
            else
                CompletedTasks.Remove(task);
        }
    }
}
