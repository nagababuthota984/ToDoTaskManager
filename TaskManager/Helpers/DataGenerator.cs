using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Models;

namespace TaskManager.Helpers
{
    public class DataGenerator
    {
        public static List<Task> CreateTasks(int count)
        {
            List<Task> tasks = new();
            for(int i = 0; i < count; i++)
            {
                tasks.Add(new Task()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Dummy task {i}",
                    Description = $"Dummy descripiton",
                    Status = (Enums.Status)(i % 3),
                    Priority = (Enums.Priority)(i % 3),
                    Category = (Enums.Category)(i % 4),
                    PercentageCompleted=0,
                    DueDate = DateTime.Now.AddHours(i%3+1),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                }) ;
            }
            return tasks;
        }
    }
}
