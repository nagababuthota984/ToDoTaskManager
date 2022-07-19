using System;
using System.Collections.Generic;
using TaskManager.Models;
using static TaskManager.Models.Enums;

namespace TaskManager.Data
{
    public interface ITaskRepository
    {
        void CreateTask(Task task);
        Models.Task GetTaskById(Guid id);
        void UpdateTask(Task taskChanges);
        List<Task> GetAllTasks(Status? status=null);
        void DeleteTaskById(Guid id);
    }
}
