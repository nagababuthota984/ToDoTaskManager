using Microsoft.EntityFrameworkCore;
using TaskManager.Common;
using TaskManager.Data.SQLite;
using TaskManager.Data.SqlServer;

namespace TaskManager.Data
{
    public static class DbContextFactory
    {
        private static string sqliteConnectionString = $"Data Source={Constant.ProjectDirectory}\\TaskManager\\Data\\SQLite\\TaskManager.db";
        public const string sqlServerConnectionString = @"Data Source=NAG-HP\SQLEXPRESS;Initial Catalog=TaskManagerDb;Integrated Security=True";
        public const string mySqlConnectionString = "Server=zjicup8yxnhs.us-east-4.psdb.cloud;Database=taskmanager;user=ay8x8ftpmaov;password=pscale_pw_EEDdu6AjXngK0eUP4QAb4E4QcFELjSpp0T_6y0p4MZQ;SslMode=VerifyFull;";
        
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
