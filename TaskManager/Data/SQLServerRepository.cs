using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Data.SqlServer;
using static TaskManager.Models.Enums;

namespace TaskManager.Data
{
    public class SqlServerRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _context;

        public SqlServerRepository()
        {
            _context = DbContextFactory.GetSqlServerDbContext();
        }

        public void CreateTask(Models.Task task)
        {
            _context.Tasks.Add(MapperBootstrapper.Mapper.Map<SqlServer.Task>(task));
            _context.SaveChanges();
        }

        public Models.Task GetTask(Guid id)
        {
            return MapperBootstrapper.Mapper.Map<Models.Task>(_context.Tasks.FirstOrDefault(tsk => tsk.Id == id));
        }

        public void DeleteTask(Guid id)
        {
            _context.Tasks.FirstOrDefault(tsk => tsk.Id == id).IsDeleted = true;
            _context.SaveChanges();
        }

        public List<Models.Task> GetTasks(Status? status = null)
        {
            return MapperBootstrapper.Mapper.Map<List<Models.Task>>(status.HasValue ? _context.Tasks.Where(tsk => tsk.Status == (int)status && tsk.IsDeleted == false) : _context.Tasks.Where(tsk => tsk.IsDeleted == false));

        }

        public void UpdateTask(Models.Task task)
        {
            var taskToUpdate = MapperBootstrapper.Mapper.Map<SqlServer.Task>(task);
            _context.SaveChanges();
        }
    }
}
