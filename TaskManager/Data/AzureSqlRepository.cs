using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Data.AzureSql;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class AzureSqlRepository : ITaskRepository
    {
        private readonly TaskManagerContext _context;

        public AzureSqlRepository()
        {
            _context = DbContextFactory.GetAzureSqlDbContext();
        }


        public void CreateTask(Models.Task task)
        {
            _context.Tasks.Add(MapperBootstrapper.Mapper.Map<AzureSql.Task>(task));
            _context.SaveChanges();
        }

        public void DeleteTask(Guid id)
        {
             _context.Tasks.FirstOrDefault(tsk => tsk.Id == id).IsDeleted=true;
            _context.SaveChanges();
        }

        public Models.Task GetTask(Guid id)
        {
            return MapperBootstrapper.Mapper.Map<Models.Task>(_context.Tasks.FirstOrDefault(tsk => tsk.Id == id));
        }

        public List<Models.Task> GetTasks(Enums.Status? status = null)
        {
            return MapperBootstrapper.Mapper.Map<List<Models.Task>>(status.HasValue ? _context.Tasks.Where(tsk => tsk.Status == (int)status && tsk.IsDeleted == false) : _context.Tasks.Where(tsk => tsk.IsDeleted == false));
        }

        public void UpdateTask(Models.Task task)
        {
            AzureSql.Task taskToUpdate = _context.Tasks.FirstOrDefault(tsk => tsk.Id == task.Id);
            taskToUpdate.Name = task.Name;
            taskToUpdate.Description = task.Description;
            taskToUpdate.Status = (int)task.Status;
            taskToUpdate.Priority = (int)task.Priority;
            taskToUpdate.Category = (int)task.Category;
            taskToUpdate.DueDate = task.DueDate;
            taskToUpdate.PercentageCompleted = (decimal?)task.PercentageCompleted;
            _context.SaveChanges();
        }
    }
}
