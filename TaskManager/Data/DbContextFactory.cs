using Microsoft.EntityFrameworkCore;
using TaskManager.Common;
using TaskManager.Data.SQLite;
using TaskManager.Data.SqlServer;

namespace TaskManager.Data
{
    public class DbContextFactory
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
            .UseSqlServer(sqlServerConnectionString)
            .Options;
            return new TaskManagerDbContext(options);
        }
    }
}
