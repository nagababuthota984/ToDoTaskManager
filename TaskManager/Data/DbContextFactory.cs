using Microsoft.EntityFrameworkCore;
using TaskManager.Data.SQLite;
using TaskManager.Data.SqlServer;
using System.Configuration;

namespace TaskManager.Data
{
    public class DbContextFactory
    {
        public const string sqliteConnectionString = @"Data Source=C:\Users\nagababu.t\source\repos\ToDoTaskManager\TaskManager\Data\SQLite\TaskManager.db";
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
            .UseSqlServer(sqlServerConnectionString)
            .Options;
            return new TaskManagerDbContext(options);
        }
    }
}
