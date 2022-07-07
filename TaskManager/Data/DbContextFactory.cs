using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.SQLite;
using TaskManager.Data.SqlServer;

namespace TaskManager.Data
{
    public class DbContextFactory
    {
        private const string sqliteConnectionString = @"Data Source=E:\Technovert Projects\TaskManager\TaskManager\Data\SQLite\TaskManager.db";
        private const string sqlServerConnectionString = @"Data Source=NAG-HP\SQLEXPRESS;Initial Catalog=TaskManagerDb;Integrated Security=True";
        public static SQLiteDbContext GetSQLiteDbContext()
        {
            var options = new DbContextOptionsBuilder<SQLiteDbContext>()
                .UseSqlite(sqliteConnectionString)
                .Options;
            return new SQLiteDbContext(options);
        }
        public static TaskManagerDbContext GetSqlServerDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseSqlServer(sqlServerConnectionString)
            .Options;
            return new TaskManagerDbContext(options);
        }
    }
}
