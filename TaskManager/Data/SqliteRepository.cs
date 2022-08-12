using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Data.SQLite;
using static TaskManager.Models.Enums;

namespace TaskManager.Data
{
    public class SqliteRepository : ITaskRepository
    {
        private readonly SQLiteDbContext _context;

        public SqliteRepository()
        {
            _context = DbContextFactory.GetSQLiteDbContext();
        }

        public void CreateTask(Models.Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public Models.Task GetTask(Guid id)
        {
            return _context.Tasks.FirstOrDefault(tsk => tsk.Id == id);
        }

        public void DeleteTask(Guid id)
        {
            _context.Tasks.FirstOrDefault(tsk => tsk.Id == id).IsDeleted = true;
            _context.SaveChanges();
        }

        public List<Models.Task> GetTasks(Status? status = null)
        {
            return status.HasValue ? _context.Tasks.Where(tsk => tsk.Status == status && !tsk.IsDeleted).ToList() : _context.Tasks.Where(tsk => !tsk.IsDeleted).ToList();
        }

        public void UpdateTask(Models.Task task)
        {
            Models.Task taskToUpdate = _context.Tasks.FirstOrDefault(tsk => tsk.Id == task.Id);
            taskToUpdate.Name = task.Name;
            taskToUpdate.Description = task.Description;
            taskToUpdate.Status = task.Status;
            taskToUpdate.IsDeleted = task.IsDeleted;
            taskToUpdate.DueDate = task.DueDate;
            taskToUpdate.Category = task.Category;
            taskToUpdate.Priority = task.Priority;
            taskToUpdate.PercentageCompleted = task.PercentageCompleted;
            _context.SaveChanges();
        }
    }
}
