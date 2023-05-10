using backend.EndpointFilters;
using backend.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using backend.Endpoints;

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

Console.WriteLine($"App is in development: {app.Environment.IsDevelopment()}");
Console.WriteLine($"Connection string: {connectionString}");
Console.WriteLine($"Current DIR: {System.IO.Directory.GetCurrentDirectory()}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

//Map endpoints
app.MapQuestionnaireEndpoints();
app.MapAnswerEndpoints();
app.MapInvitationEndpoints();
app.MapQuestionEndpoints();


//////// Testing ////////
app.MapGet("/hello", () =>
{
    return Results.Ok(new { 
        message = "hello world",
        path = System.IO.Directory.GetCurrentDirectory(), 
        time= DateTime.Now.ToString("yyyy-MM-dd")});
}).WithTags("Testing");

app.MapGet("/secured", () =>
{
    return "Endpoint secured by firebase";
}).WithTags("Testing").AddEndpointFilter<FirebaseAuthFilter>();

app.Run();