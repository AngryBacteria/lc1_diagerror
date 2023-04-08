using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    class DiagErrorDb : DbContext
    {
        public DbSet<Questionnaire> Questionnaires { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;

        public string DbPath { get; }

        public DiagErrorDb()
        {
            var projectPath = AppDomain.CurrentDomain.BaseDirectory;

            //if "bin" is present, remove all the path starting from "bin" word. Path is now the project root folder
            if (projectPath.Contains("bin"))
            {
                int index = projectPath.IndexOf("bin");
                projectPath = projectPath.Substring(0, index);
            }

            var dbPath = Path.Combine(projectPath, "Database", "diagError.db");
            DbPath = dbPath;
        }

        // The following configures EF to create a Sqlite database file in the "Database" folder inside the project
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
    public class Questionnaire
    {
        public int Id { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
    }
}
