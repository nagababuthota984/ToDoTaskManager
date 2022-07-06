using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TaskManager.Models;

namespace TaskManager.Data.SQLite
{
    public  class SQLiteDbContext : DbContext
    {
        public SQLiteDbContext()
        {
        }

        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options)
            : base(options)
        {
        }

        public  DbSet<TaskDisplayModel> Tasks { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=E:\\Technovert Projects\\TaskManager\\TaskManager\\Data\\SQLite\\TaskManager.db");
            }
        }

        
    }
}
