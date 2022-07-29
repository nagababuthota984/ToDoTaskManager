﻿using System;
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

        public SqlServerRepository()
        {
            _context = DbContextFactory.GetSqlServerDbContext();
        }

        public void CreateTask(Models.Task task)
        {
            try
            {
                _context.Tasks.Add(MapperHelper.Mapper.Map<SqlServer.Task>(task));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Models.Task GetTask(Guid id)
        {
            return MapperHelper.Mapper.Map<Models.Task>(_context.Tasks.FirstOrDefault(tsk => tsk.Id == id));
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
            return MapperHelper.Mapper.Map<List<Models.Task>>(status.HasValue ? _context.Tasks.Where(tsk => tsk.Status == (int)status && tsk.IsDeleted == false) : _context.Tasks.Where(tsk => tsk.IsDeleted == false));

        }

        public void UpdateTask(Models.Task task)
        {
            try
            {
                var taskToUpdate = MapperHelper.Mapper.Map<SqlServer.Task>(task);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
