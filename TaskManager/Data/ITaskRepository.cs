﻿using System;
using System.Collections.Generic;
using TaskManager.Models;
using static TaskManager.Models.Enums;

namespace TaskManager.Data
{
    public interface ITaskRepository
    {
        void CreateTask(Task task);

        Models.Task GetTask(Guid id);

        void UpdateTask(Task taskChanges);

        List<Task> GetTasks(Status? status = null);

        void DeleteTask(Guid id);
    }
}
