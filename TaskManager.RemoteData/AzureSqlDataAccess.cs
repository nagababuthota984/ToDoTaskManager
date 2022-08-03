using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TaskManager.Models;
using static TaskManager.Models.Enums;

namespace TaskManager.RemoteData
{
    public class AzureSqlDataAccess 
    {
        private readonly TaskManagerContext _dbContext;

        public AzureSqlDataAccess()
        {
            _dbContext = BuildDbContext();
        }

        public TaskManagerContext BuildDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskManagerContext>()
            .UseSqlServer(ConfigurationManager.ConnectionStrings["Default"].ConnectionString)
            .Options;
            return new TaskManagerContext(options);
        }

    }
}
