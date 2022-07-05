using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class SQLServerRepository : ITaskRepository
    {
        private readonly ToDoTaskManagerContext _context;
        private readonly IMapper _mapper;

        public SQLServerRepository(ToDoTaskManagerContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void CreateTask(TaskDisplayModel task)
        {
            _context.Tasks.Add(_mapper.Map<Task>(task));
            _context.SaveChanges();
        }

        public void DeleteTaskById(Guid id)
        {
            _context.Tasks.Remove(_context.Tasks.FirstOrDefault(tsk => tsk.Id == id));
            _context.SaveChanges();
        }
        public  List<TaskDisplayModel> GetAllTasks()
        {
            return _mapper.Map<List<TaskDisplayModel>>(_context.Tasks);
        }

        public void UpdateTask(TaskDisplayModel taskChanges)
        {
            var task = _context.Tasks.Attach(_mapper.Map<Task>(taskChanges));
            task.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
