using System;
using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.Data
{
    public interface ITaskRepository
    {
        void CreateTask(Task task);
        void UpdateTask(Task taskChanges);
        List<Task> GetAllTasks();
        Task GetTaskById(Guid id);

    }
}
