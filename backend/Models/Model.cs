using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
    class DiagErrorDb : DbContext
    {
        public DbSet<Questionnaire> Questionnaires { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Option> Options { get; set; } = null!;

        public string DbPath { get; }

        public DiagErrorDb(DbContextOptions options): base (options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   
            modelBuilder.Entity<Answer>()
                .ToTable(b => b.HasCheckConstraint("CHK_InvitationIdLength", "LENGTH(InvitationId) = 8"));

            modelBuilder.Entity<Questionnaire>()
                .ToTable(b => b.HasCheckConstraint("CHK_IdentifierLength", "LENGTH(Identifier) = 2"));
        }

        /* Todo auto increment the index for each question grouped by questionnaire
         * WORK IN PROGRESS
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Get the added questions
            var addedQuestions = ChangeTracker.Entries<Question>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity);

            // Group the added questions by questionnaire
            var groups = addedQuestions.GroupBy(q => q.QuestionnaireId);

            // Calculate the index for each question in each group
            foreach (var group in groups)
            {
                int index = Questions
                    .Where(q => q.QuestionnaireId == group.Key)
                    .Select(q => q.Index)
                    .DefaultIfEmpty(0)
                    .Max() + 1;
                foreach (var question in group.OrderBy(q => q.QuestionId))
                {
                    question.Index = index++;
                }
            }

            // Get the deleted questions
            var deletedQuestions = ChangeTracker.Entries<Question>()
                .Where(e => e.State == EntityState.Deleted)
                .Select(e => e.Entity);

            // Update the index of the remaining questions in each questionnaire
            foreach (var deletedQuestion in deletedQuestions)
            {
                var questions = Questions
                    .Where(q => q.QuestionnaireId == deletedQuestion.QuestionnaireId && q.Index > deletedQuestion.Index)
                    .OrderBy(q => q.Index)
                    .ToList();

                foreach (var question in questions)
                {
                    question.Index--;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        */
    }

    [JsonConverter(typeof(EnumConverter<Language>))]
    public enum Language
    {
        EN,
        DE,
        FR
    }

    [JsonConverter(typeof(EnumConverter<QuestionType>))]
    public enum QuestionType
    {
        Likert,
        FreeText,
        SingleChoice,
        MultipleChoice
    }

    [Index(nameof(Identifier), nameof(Language), IsUnique = true)]
    public class Questionnaire
    {
        public int QuestionnaireId { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        [MaxLength(2)]
        public string Identifier { get; set; }
        public Language Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionForCustomer { get; set; }
        public int ValidAfterDays { get; set; }
        public int ValidForDays { get; set; }
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public int QuestionnaireId { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
        [JsonIgnore]
        public Questionnaire Questionnaire { get; set; }
        public string Text { get; set; }
        public string Subtext { get; set; }
        public bool Optional { get; set; }
        public QuestionType Questiontype { get; set; }
        public List<Option> Options { get; set; } = new List<Option>();
        public int Index { get; set; }
    }

    public class Answer
    {
        public int AnswerId { get; set; }
        [JsonIgnore]
        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public DateOnly Date { get; set; }
        public string InvitationId { get; set; }
    }

    public class Option
    {
        public int OptionId { get; set; }
        public int Index { get; set; }
        public String Value { get; set; }
    }
}
