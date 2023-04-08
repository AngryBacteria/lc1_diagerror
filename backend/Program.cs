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
builder.Services.AddDbContext<DiagErrorDb>();

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

app.MapGet("/question", async (DiagErrorDb db) => await db.Questions.ToListAsync());


app.MapPost("/question", async (DiagErrorDb db, Question question) =>
{
    await db.Questions.AddAsync(question);
    await db.SaveChangesAsync();
    return Results.Created($"/question/{question.Id}", question);
});


app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
