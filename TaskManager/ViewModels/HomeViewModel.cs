﻿using Caliburn.Micro;
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
        private DateTime _dueDate;
        private string _name;
        private Task _selectedTask;
        private string _description;
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
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(nameof(Name)); }
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
        }
        public void CreateTask()
        {
            if (SelectedStatus == Status.New)
            {
                NewTasks.Add(new(Name, Description, SelectedStatus, SelectedPriority, DueDate));
            }
            else if (SelectedStatus == Status.InProgress)
            {
                InProgressTasks.Add(new(Name, Description, SelectedStatus, SelectedPriority, DueDate));
            }
            else
            {
                CompletedTasks.Add(new(Name, Description, SelectedStatus, SelectedPriority, DueDate));
            }

            ResetInputControls();
        }

        private void ResetInputControls()
        {
            Name = string.Empty;
            Description = string.Empty;
            SelectedPriority = Priority.Low;
            SelectedStatus = Status.New;
        }

        public void Cancel()
        {
            ResetInputControls();
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
        public void DeleteById(Guid id, Status status)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to delete the task?", "Delete Task", MessageBoxButton.YesNo))
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
                    MessageBox.Show("Couldn't delete the task. Please try again", "Delete Unsuccessful");
                }

            }
        }
        public void ShowSelectedTask()
        {
            if (SelectedTask != null)
            {
                Name = SelectedTask.Name;
                Description = SelectedTask.Description;
                SelectedStatus = SelectedTask.Status;
                SelectedPriority = SelectedTask.Priority;
                DueDate = SelectedTask.DueDate;
            }
            else
                ResetInputControls();
        }
    }
}
