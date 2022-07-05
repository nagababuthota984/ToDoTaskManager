using System;
using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.Data
{
    public interface ITaskRepository
    {
        void CreateTask(TaskDisplayModel task);
        void UpdateTask(TaskDisplayModel taskChanges);
        List<TaskDisplayModel> GetAllTasks();
        void DeleteTaskById(Guid id);
    }
}
