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
                throw;
            }
        }

        public Models.Task GetTask(Guid id)
        {
            return _context.Tasks.FirstOrDefault(tsk => tsk.Id == id);
        }

        public void DeleteTask(Guid id)
        {
            try
            {
                _context.Tasks.FirstOrDefault(tsk => tsk.Id == id).IsDeleted = true;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Models.Task> GetTasks(Status? status = null)
        {
            return status.HasValue ? _context.Tasks.Where(tsk => tsk.Status == status && !tsk.IsDeleted ).ToList() : _context.Tasks.Where(tsk => !tsk.IsDeleted).ToList();
        }

        public void UpdateTask(Models.Task task)
        {
            try
            {
                var taskToUpdate = _context.Tasks.FirstOrDefault(tsk => tsk.Id == task.Id);
                taskToUpdate = MapperBootstrapper.Mapper.Map<Models.Task>(task);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
