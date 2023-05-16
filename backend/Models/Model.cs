using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
    /// <summary>
    /// DBContext for the main database
    /// </summary>
    class DiagErrorDb : DbContext
    {
        public DbSet<Questionnaire> Questionnaires { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Option> Options { get; set; } = null!;

        public string DbPath { get; }

        public DiagErrorDb(DbContextOptions options): base (options)
        {
            var dbPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Database", "diagError.db");
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
    }

    /// <summary>
    /// ENUM for the supported languages in the frontend-apps
    /// </summary>
    [JsonConverter(typeof(EnumConverter<Language>))]
    public enum Language
    {
        EN,
        DE,
        FR
    }

    /// <summary>
    /// ENUM for the supported question-types of the frontend and backend
    /// </summary>
    [JsonConverter(typeof(EnumConverter<QuestionType>))]
    public enum QuestionType
    {
        Likert,
        FreeText,
        SingleChoice,
        MultipleChoice
    }

    /// <summary>
    /// Questionnaire class. Used to define questionnaires for the frontend
    /// </summary>
    [Index(nameof(Identifier), nameof(Language), IsUnique = true)]
    [Index(nameof(Identifier))]
    [Index(nameof(Language))]
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

    /// <summary>
    /// Question class used to define questions of a questionnaire. Part of the questionnaire class
    /// </summary>
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

    /// <summary>
    /// Anser class used to define answers to a question. Part of the answer class
    /// </summary>
    [Index(nameof(Date))]
    [Index(nameof(InvitationId))]
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

    /// <summary>
    /// Option class used to define options of an answer. Part of the answer class but optional
    /// </summary>
    public class Option
    {
        public int OptionId { get; set; }
        public int Index { get; set; }
        public String Value { get; set; }
    }
}
