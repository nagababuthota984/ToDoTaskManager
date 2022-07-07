using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.SQLite;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class SqliteRepository : ITaskRepository
    {
        private readonly SQLiteDbContext _context;

        public SqliteRepository()
        {
            _context = DbContextFactory.GetSQLiteDbContext();
        }
        public void CreateTask(TaskDisplayModel task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void DeleteTaskById(Guid id)
        {
            _context.Tasks.FirstOrDefault(tsk => tsk.Id == id).IsDeleted = true;
            _context.SaveChanges();
        }

        public List<TaskDisplayModel> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }
        public void UpdateTask(TaskDisplayModel taskChanges)
        {
            _context.SaveChanges();
        }
    }
}
