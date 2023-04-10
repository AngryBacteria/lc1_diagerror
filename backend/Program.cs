using backend.EndpointFilters;
using backend.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Firebase
FirebaseApp firebaseApp = FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("Data\\firebaseServiceAccount.json"),
});
FirebaseAuth firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);

//Dependency Injection as Singleton
builder.Services.AddSingleton(firebaseApp);
builder.Services.AddSingleton(firebaseAuth);

//Sqlite database
var connectionString = builder.Configuration.GetConnectionString("diagError") ?? "Data Source=diagError.db";
builder.Services.AddSqlite<DiagErrorDb>(connectionString);

//cors
builder.Services.AddCors();

var app = builder.Build();

//cors
app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.SetIsOriginAllowed((host) => true)
.AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello", () =>
{
    return "Hello World";
});

app.MapGet("/secured", () =>
{
    return "Endpoint secured by firebase";
}).AddEndpointFilter<FirebaseAuthFilter>();

//Endpoints for GET all Questionnaires with answers
app.MapGet("/questionnaire/complete", async (DiagErrorDb db) => await db.Questionnaires.Include(q => q.Questions).ThenInclude(q => q.Answers).ToListAsync());

//Endpoints for GET all Questionnaires with answers and filtering possibilities
app.MapGet("/questionnaire/complete/filter", async (DiagErrorDb db, string? id, string? identifier, string? language) => 
{
    var questionnaires = db.Questionnaires
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .AsQueryable();

    if (!string.IsNullOrEmpty(id))
    {
        questionnaires = questionnaires.Where(q => q.QuestionnaireId == int.Parse(id));
    }

    if (!string.IsNullOrEmpty(identifier))
    {
        questionnaires = questionnaires.Where(q => q.Identifier == identifier);
    }

    if (!string.IsNullOrEmpty(language))
    {
        questionnaires = questionnaires.Where(q => q.Language == Enum.Parse<Language>(language)); //Todo: try/catch parsing
    }

    return await questionnaires.ToListAsync();
});

//Endpoints for GET Questionnaires without answers
app.MapGet("/questionnaire/light", async (DiagErrorDb db) => await db.Questionnaires.Include(q => q.Questions).ToListAsync());

//Endpoints for POST Questionnaires without answers
app.MapPost("/questionnaire/light", async (DiagErrorDb db, Questionnaire questionnaire) =>
{
    await db.Questionnaires.AddAsync(questionnaire);
    await db.SaveChangesAsync();

    return Results.Created($"/questionnaire/light/{questionnaire.QuestionnaireId}", questionnaire);
});

//Endpoints for GET Questions with answers
app.MapGet("/question/complete", async (DiagErrorDb db) => await db.Questions.Include(a => a.Answers).ToListAsync());

//Endpoints for GET Questions without answers
app.MapGet("/question/light", async (DiagErrorDb db) => await db.Questions.ToListAsync());

//Endpoints for POST Questions without answers
app.MapPost("/question/light", async (DiagErrorDb db, Question question) =>
{
    await db.Questions.AddAsync(question);
    await db.SaveChangesAsync();
    return Results.Created($"/question/light/{question.QuestionId}", question);
});

//Endpoints for GET answers
app.MapGet("/answer", async (DiagErrorDb db) => await db.Answers.ToListAsync());

//Endpoints for POST answers
app.MapPost("/answer", async (DiagErrorDb db, Answer[] answers) =>
{
    await db.Answers.AddRangeAsync(answers);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();