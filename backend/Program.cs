using backend.EndpointFilters;
using backend.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/hello", () =>
{
    return "Hello World";
});

app.MapGet("/secured", () =>
{
    return "Endpoint secured by firebase";
}).AddEndpointFilter<FirebaseAuthFilter>();

//Endpoints for Questionnaires with answers
app.MapGet("/questionnaire/complete", async (DiagErrorDb db) => await db.Questionnaires.Include(q => q.Questions).ThenInclude(q => q.Answers).ToListAsync()) ;

app.MapGet("/questionnaire/complete/filter", async (DiagErrorDb db, string id, string identifier, string language) => 
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

//Endpoints for questionnaires without answers
app.MapGet("/questionnaire/light", async (DiagErrorDb db) => await db.Questionnaires.Include(q => q.Questions).ToListAsync());

app.MapPost("/questionnaire/light", async (DiagErrorDb db, Questionnaire questionnaire) =>
{
    await db.Questionnaires.AddAsync(questionnaire);
    await db.SaveChangesAsync();

    return Results.Created($"/questionnaire/light/{questionnaire.QuestionnaireId}", questionnaire);
});

//Endpoints for questions with answers
app.MapGet("/question/complete", async (DiagErrorDb db) => await db.Questions.Include(a => a.Answers).ToListAsync());

//Endpoints for questions without answers
app.MapGet("/question/light", async (DiagErrorDb db) => await db.Questions.ToListAsync());

app.MapPost("/question/light", async (DiagErrorDb db, Question question) =>
{
    await db.Questions.AddAsync(question);
    await db.SaveChangesAsync();
    return Results.Created($"/question/light/{question.QuestionId}", question);
});

//Endpoints for answers
app.MapGet("/answer", async (DiagErrorDb db) => await db.Answers.ToListAsync());

app.MapPost("/answer", async (DiagErrorDb db, Answer answer) =>
{
    await db.Answers.AddAsync(answer);
    await db.SaveChangesAsync();
    return Results.Created($"/answer/{answer.AnswerId}", answer);
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
