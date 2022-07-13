using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data.SqlServer;
using static TaskManager.Models.Enums;

namespace TaskManager.Data
{
    public class SqlServerRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;

        public SqlServerRepository(IMapper mapper)
        {
            _context = DbContextFactory.GetSqlServerDbContext();
            _mapper = mapper;
        }
        public void CreateTask(Models.Task task)
        {
            try
            {
                _context.Tasks.Add(_mapper.Map<SqlServer.Task>(task));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
        }
        public Models.Task GetTaskById(Guid id)
        {
            return _mapper.Map<Models.Task>(_context.Tasks.FirstOrDefault(tsk => tsk.Id == id));
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
            return _mapper.Map<List<Models.Task>>(status.HasValue ? _context.Tasks.Where(tsk => tsk.Status == (int)status && tsk.IsDeleted==false) : _context.Tasks.Where(tsk=>tsk.IsDeleted==false));

        }

        public void UpdateTask(Models.Task taskChanges)
        {
            try
            {
                var task = _mapper.Map<SqlServer.Task>(taskChanges);
                _context.Tasks.Remove(_context.Tasks.FirstOrDefault(tsk => tsk.Id == task.Id));
                _context.Tasks.Add(task);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
        }
    }


}
