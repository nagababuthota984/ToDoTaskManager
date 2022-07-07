using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Data.SQLite;

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

        public void DeleteTaskById(Guid id)
        {
            _context.Tasks.FirstOrDefault(tsk => tsk.Id == id).IsDeleted = true;
            _context.SaveChanges();
        }

        public List<Models.Task> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }
        public void UpdateTask(Models.Task taskChanges)
        {
            _context.SaveChanges();
        }
    }
}
