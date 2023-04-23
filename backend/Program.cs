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


//////// Answer ////////
//Endpoints for GET answers
app.MapGet("/answer", async (DiagErrorDb db) => 
{
    return await db.Answers.ToListAsync();
}).WithTags("Answer");

//Endpoints for POST answers
app.MapPost("/answer", async (DiagErrorDb db, Answer[] answers) =>
{
    await db.Answers.AddRangeAsync(answers);
    await db.SaveChangesAsync();
    return Results.Ok();
}).WithTags("Answer");

//////// Invitation ////////
//Endpoints for POST Invitation code
app.MapPost("/invitation", async (DiagErrorDb db, string invitationCode, string? language) => 
{
    string identifier = invitationCode[..2];//Extract Identifier out of invitationCode

    //Searching for questionnaires with matching identifier
    var questionnaires = db.Questionnaires
         .Include(q => q.Questions)
         .ThenInclude(q => q.Options)
         .Where(q => q.Identifier == identifier)
         .AsQueryable();

    //if no questionnaire found
    if (questionnaires == null || !questionnaires.Any())
    {
        return Results.NotFound("No Questionnaire found with this invitation code");
    }

    //filter by language
    var filteredQuestionnaires = questionnaires.Where(q => language == null || Enum.IsDefined(typeof(Language), language.ToUpper()) && q.Language == Enum.Parse<Language>(language.ToUpper(), true));

    //return all found questionnaires or exact match by language
    var result = filteredQuestionnaires.Any() ? await filteredQuestionnaires.ToListAsync() : await questionnaires.ToListAsync();
    return Results.Ok(result);
}).WithTags("Invitation");

//////// Question-Complete ////////
//Endpoints for GET Questions with answers
app.MapGet("/question/complete", async (DiagErrorDb db) => 
{
    return await db.Questions.Include(a => a.Answers).ToListAsync();
}).WithTags("Question-Complete");

//////// Question-Light ////////
//Endpoints for GET Questions without answers
app.MapGet("/question/light", async (DiagErrorDb db) => 
{
    return await db.Questions.ToListAsync();
}).WithTags("Question-Light");

//Endpoints for POST Questions without answers
app.MapPost("/question/light", async (DiagErrorDb db, Question question) =>
{
    await db.Questions.AddAsync(question);
    await db.SaveChangesAsync();
    return Results.Created($"/question/light/{question.QuestionId}", question);
}).WithTags("Question-Light");

//////// Questionnaire-Complete ////////
//Endpoints for GET all Questionnaires with answers
app.MapGet("/questionnaire/complete", async (DiagErrorDb db) => 
{
    return await db.Questionnaires.Include(q => q.Questions).ThenInclude(q => q.Answers).ToListAsync();
}).WithTags("Questionnaire-Complete");

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
}).WithTags("Questionnaire-Complete");

//////// Questionnaire-Light ////////
//Endpoints for GET Questionnaires without answers
app.MapGet("/questionnaire/light", async (DiagErrorDb db) => 
{
    return await db.Questionnaires.Include(q => q.Questions).ThenInclude(q => q.Options).ToListAsync();
}).WithTags("Questionnaire-Light");

//Endpoints for POST Questionnaires without answers
app.MapPost("/questionnaire/light", async (DiagErrorDb db, Questionnaire[] questionnaires) =>
{
    await db.Questionnaires.AddRangeAsync(questionnaires);
    await db.SaveChangesAsync();

    return Results.Ok();
}).WithTags("Questionnaire-Light");

//////// Testing ////////
app.MapGet("/hello", () =>
{
    return "Hello World";
}).WithTags("Testing");

app.MapGet("/secured", () =>
{
    return "Endpoint secured by firebase";
}).WithTags("Testing").AddEndpointFilter<FirebaseAuthFilter>();

app.Run();