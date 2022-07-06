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
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;

        public SqliteRepository(TaskManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void CreateTask(TaskDisplayModel task)
        {
            _context.Tasks.Add(_mapper.Map<SQLite.Task>(task));
            _context.SaveChanges();
        }

        public void DeleteTaskById(Guid id)
        {
            _context.Tasks.FirstOrDefault(tsk => tsk.Id == id.ToString()).IsDeleted = 1;
            _context.SaveChanges();
        }

        public List<TaskDisplayModel> GetAllTasks()
        {
            return _mapper.Map<List<TaskDisplayModel>>(_context.Tasks);
        }

        public void UpdateTask(TaskDisplayModel taskChanges)
        {
            var task = _mapper.Map<SQLite.Task>(taskChanges);
            _context.Tasks.Remove(_context.Tasks.FirstOrDefault(tsk => tsk.Id == task.Id));
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }
    }
}
