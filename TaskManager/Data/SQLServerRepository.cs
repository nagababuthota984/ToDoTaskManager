using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Data.SqlServer;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class SqlServerRepository:ITaskRepository
    {
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;

        public SqlServerRepository(IMapper mapper)
        {
            _context = DbContextFactory.GetSqlServerDbContext();
            _mapper = mapper;
        }
        public void CreateTask(TaskDisplayModel task)
        {
            _context.Tasks.Add(_mapper.Map<Task>(task));
            _context.SaveChanges();
        }

        public void DeleteTaskById(Guid id)
        {
            _context.Tasks.FirstOrDefault(tsk => tsk.Id == id).IsDeleted = true ;
            _context.SaveChanges();
        }
        public List<TaskDisplayModel> GetAllTasks()
        {
            return _mapper.Map<List<TaskDisplayModel>>(_context.Tasks);
        }

        public void UpdateTask(TaskDisplayModel taskChanges)
        {
            var task = _mapper.Map<Task>(taskChanges);
            _context.Tasks.Remove(_context.Tasks.FirstOrDefault(tsk => tsk.Id == task.Id));
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }
    }


}
