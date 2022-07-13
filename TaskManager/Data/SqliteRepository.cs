using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaskManager.Common;
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
            try
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
        }
        public Models.Task GetTaskById(Guid id)
        {
            return _context.Tasks.FirstOrDefault(tsk => tsk.Id == id);
        }

        public void DeleteTaskById(Guid id)
        {
            try
            {
                _context.Tasks.FirstOrDefault(tsk => tsk.Id == id).IsDeleted = true;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
        }

        public List<Models.Task> GetAllTasks(Status? status = null)
        {
            return status.HasValue ? _context.Tasks.Where(tsk => tsk.Status == status && tsk.IsDeleted == false).ToList() : _context.Tasks.Where(tsk => tsk.IsDeleted == false).ToList();
        }
        public void UpdateTask(Models.Task taskChanges)
        {
            try
            {
                var task = _context.Tasks.FirstOrDefault(tsk => tsk.Id == taskChanges.Id);
                task.Name = taskChanges.Name;
                task.Description = taskChanges.Description;
                task.Status = taskChanges.Status;
                task.Priority = taskChanges.Priority;
                task.PercentageCompleted = taskChanges.PercentageCompleted;
                task.Category = taskChanges.Category;
                task.DueDate = taskChanges.DueDate;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
        }
    }
}
