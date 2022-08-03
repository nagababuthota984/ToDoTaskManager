using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TaskManager.Common;
using TaskManager.Data.AzureSql;
using TaskManager.Data.SQLite;
using TaskManager.Data.SqlServer;

namespace TaskManager.Data
{
    public static class DbContextFactory
    {
        public static string sqliteConnectionString = $"Data Source={Constant.ProjectDirectory}\\TaskManager\\Data\\SQLite\\TaskManager.db";
        public const string sqlServerConnectionString = @"Data Source=NAG-HP\SQLEXPRESS;Initial Catalog=TaskManagerDb;Integrated Security=True";
       
        
        public static ITaskRepository TaskRepository { get; set; }


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
            .UseSqlServer(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString)
            .Options;
            return new TaskManagerDbContext(options);
        }

        public static TaskManagerContext GetAzureSqlDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskManagerContext>()
            .UseSqlServer(ConfigurationManager.ConnectionStrings["AzureSql"].ConnectionString)
            .Options;
            return new TaskManagerContext(options);
        }
    }
}
