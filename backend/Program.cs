using backend.EndpointFilters;
using backend.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
app.MapGet("/answer/filter", async (DiagErrorDb db, string? questionnaireIdentifier) =>
{
    //Retrieving all stored answers
    var answers = db.Answers
            .AsQueryable();

    //Filtering answers with given identifier
    if (!string.IsNullOrEmpty(questionnaireIdentifier))
    {
        answers = answers.Where(q => q.InvitationId.StartsWith(questionnaireIdentifier.ToUpper()));
    }

    return Results.Ok(await db.Answers.ToListAsync());

}).WithOpenApi(operation => new(operation)
{
    Summary = "Get answers with filtering",
    Description = "This endpoint retrieves all stored answers. You can filter for answers belonging to certain questionnaires with its identifier."
}).WithTags("Answer");

//Endpoints for POST answers
app.MapPost("/answer", async (DiagErrorDb db, Answer[] answers) =>
{
    try
    {
        if (answers == null || !answers.Any())
        {
            return Results.BadRequest("The request does not contain any answers.");
        }

        await db.Answers.AddRangeAsync(answers);
        await db.SaveChangesAsync();

        return Results.Ok($"{answers.Length} answers stored to the database");
    }
    catch (Exception e)
    {
        return Results.Problem(
           statusCode: StatusCodes.Status500InternalServerError,
           detail: e.Message
        );
    }
}).WithOpenApi(operation => new(operation)
{
    Summary = "Store new answers",
    Description = "This endpoint stores new answers in the database."
}).WithTags("Answer");

//////// Invitation ////////
//Endpoints for POST Invitation code
app.MapPost("/invitation", async (DiagErrorDb db, string invitationCode, string? language) => 
{
    try
    {
        if(invitationCode.Length <2 )
        {
            throw new ArgumentException("The identifier is too short. It has to consist of 2 digits representing a questionnaire identifier and 6 digits representing unique invitation");
        };

        string identifier = invitationCode[..2];//Extract Identifier out of invitationCode

        //Searching for questionnaires with matching identifier
        var questionnaires = await db.Questionnaires
             .Include(q => q.Questions)
             .ThenInclude(q => q.Options)
             .Where(q => q.Identifier == identifier)
             .ToListAsync();

        //if no questionnaire found
        if (questionnaires == null || !questionnaires.Any())
        {
            return Results.NotFound("No Questionnaire found with this invitation code");
        }

        //filter by language
        var filteredQuestionnaires = questionnaires.Where(q => 
        language == null || Enum.IsDefined(typeof(Language), language.ToUpper()) && q.Language == Enum.Parse<Language>(language.ToUpper(), true));

        //return all found questionnaires or exact match by language
        var result = filteredQuestionnaires.Any() ? filteredQuestionnaires :  questionnaires;
        return Results.Ok(result);
    }
    catch (Exception e)
    {
        return Results.Problem(
            statusCode : StatusCodes.Status500InternalServerError,
            detail : e.Message
        );
    }
}).WithOpenApi(operation => new(operation)
{
    Summary = "Retrieve questionnaires by invitation code",
    Description = "This endpoint validates an invitation code and returns the matching questionnaires. If a language parameter is provided, the endpoint additionally returns only questionnaires that match the specified language."
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
}).WithOpenApi(operation => new(operation)
{
    Summary = "Get all questionnaires",
    Description = "This endpoint retrieves all questionnaires with their associated questions and stored answers."
}).WithTags("Questionnaire-Complete");

//Endpoints for GET all Questionnaires with answers and filtering possibilities
app.MapGet("/questionnaire/complete/filter", async (DiagErrorDb db, int? page, int? pageSize, string? id, string? identifier, string? language) => 
{
    // Calculate the number of items to skip and take based on the page and pageSize parameters
    int skip = (page.GetValueOrDefault(1) - 1) * pageSize.GetValueOrDefault(10);
    int take = pageSize.GetValueOrDefault(10);

    //Retrieving all stored questionnaires with questions and answers
    var questionnaires = db.Questionnaires
            .Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .AsQueryable();

    //Filtering questionnaires with given id
    if (!string.IsNullOrEmpty(id))
    {
        questionnaires = questionnaires.Where(q => q.QuestionnaireId == int.Parse(id));
    }

    //Filtering questionnaires with given identifier
    if (!string.IsNullOrEmpty(identifier))
    {
        questionnaires = questionnaires.Where(q => q.Identifier == identifier);
    }

    //Filtering questionnaires with given language
    if (!string.IsNullOrEmpty(language))
    {
        //checking if given language is supported in the databaseContext
        try
        {
            Enum.Parse<Language>(language);
        }
        catch (Exception e)
        {
            return Results.Problem(
                statusCode : StatusCodes.Status400BadRequest,
                detail : $"Exception Message: '{e.Message}'. Invalid language value: '{language}'. Supported Languages are: {string.Join(", ", Enum.GetNames(typeof(Language)))}.");
        }

        questionnaires = questionnaires.Where(q => q.Language == Enum.Parse<Language>(language));
    }

    // Apply pagination
    questionnaires = questionnaires.Skip(skip).Take(take);

    return Results.Ok(await questionnaires.ToListAsync());

}).WithOpenApi(operation => new(operation)
{
    Summary = "Get all questionnaires with filtering",
    Description = "This endpoint retrieves all questionnaires with their associated questions and stored answers. You can filter the list with id, identifier and language of the wished questionnaire. The list is paginated with page and pageSize." +
    "<br><br>" + //two breaks included in the text
    "e.g. There are 10 questionnaires, but my page can only handle 3 at once. If the endpoint es called from page number 2, there will be the second three questionnaires returned."
}).WithTags("Questionnaire-Complete");

//////// Questionnaire-Light ////////
//Endpoints for GET Questionnaires without answers
app.MapGet("/questionnaire/light/filter", async (DiagErrorDb db, int? page, int? pageSize, string? id, string? identifier, string? language) => 
{
    // Calculate the number of items to skip and take based on the page and pageSize parameters
    int skip = (page.GetValueOrDefault(1) - 1) * pageSize.GetValueOrDefault(10);
    int take = pageSize.GetValueOrDefault(10);

    //Retrieving all stored questionnaires with questions and answers
    var questionnaires = db.Questionnaires
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .AsQueryable();

    //Filtering questionnaires with given id
    if (!string.IsNullOrEmpty(id))
    {
        questionnaires = questionnaires.Where(q => q.QuestionnaireId == int.Parse(id));
    }

    //Filtering questionnaires with given identifier
    if (!string.IsNullOrEmpty(identifier))
    {
        questionnaires = questionnaires.Where(q => q.Identifier == identifier);
    }

    //Filtering questionnaires with given language
    if (!string.IsNullOrEmpty(language))
    {
        //checking if given language is supported in the databaseContext
        try
        {
            Enum.Parse<Language>(language);
        }
        catch (Exception e)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"Exception Message: '{e.Message}'. Invalid language value: '{language}'. Supported Languages are: {string.Join(", ", Enum.GetNames(typeof(Language)))}.");
        }

        questionnaires = questionnaires.Where(q => q.Language == Enum.Parse<Language>(language));
    }

    // Apply pagination
    questionnaires = questionnaires.Skip(skip).Take(take);

    return Results.Ok(await questionnaires.ToListAsync());
}).WithTags("Questionnaire-Light");

//Endpoints for POST Questionnaires without answers
app.MapPost("/questionnaire/light", async (DiagErrorDb db, Questionnaire[] questionnaires) =>
{
    await db.Questionnaires.AddRangeAsync(questionnaires);
    await db.SaveChangesAsync();

    return Results.Ok();
}).WithOpenApi(operation => new(operation)
{
    Summary = "Get all questionnaires with filtering",
    Description = "This endpoint retrieves all questionnaires with their associated questions. You can filter the list with id, identifier and language of the wished questionnaire. The list is paginated with page and pageSize." +
    "<br><br>" + //two breaks included in the text
    "e.g. There are 10 questionnaires, but my page can only handle 3 at once. If the endpoint es called from page number 2, there will be the second three questionnaires returned."
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