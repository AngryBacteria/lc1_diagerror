using backend.EndpointFilters;
using backend.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);

////Swagger documentation initialization
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////Firebase initialization. Creates the firebase object and adds the admin claim to the provided users
FirebaseApp firebaseApp = FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("Data\\firebaseServiceAccount.json"),
});
FirebaseAuth firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);

//Firebase admin claim
var claims = new Dictionary<string, object>()
{
    { "admin", true },
};

//List of firebase uid's to add the admin claim to
List<string> accounts = new List<string>() {"ZqEThJkICmMuxgUku53fqnuuSoV2"};

//Adds the admin claim to the admin-user accounts
foreach (string account in accounts)
{
    await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(account, claims);
    Console.WriteLine($"Claim [ADMIN] added to the user [{account}]");
}

//Dependency Injection as Singleton
builder.Services.AddSingleton(firebaseApp);
builder.Services.AddSingleton(firebaseAuth);

////Sqlite database initialization
var connectionString = builder.Configuration.GetConnectionString("diagError") ?? "Data Source=diagError.db";
builder.Services.AddSqlite<DiagErrorDb>(connectionString);

////CORS initialization
builder.Services.AddCors();

////build app
var app = builder.Build();

////CORS configuration
app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.SetIsOriginAllowed((host) => true)
.AllowCredentials());

////Status messages
Console.WriteLine($"App is in development: {app.Environment.IsDevelopment()}");
Console.WriteLine($"Connection string: {connectionString}");
Console.WriteLine($"Current DIR: {System.IO.Directory.GetCurrentDirectory()}");

////Configure the HTTP request pipeline. Swagger and https-redirection is only generated if the app is in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

////Map endpoints
app.MapQuestionnaireEndpoints();
app.MapAnswerEndpoints();
app.MapInvitationEndpoints();
app.MapUnusedEndpoints();

app.Run();