using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace backend.Endpoints
{
    public static class QuestionnaireEndpointsExt
    {
        public static void MapQuestionnaireEndpoints(this WebApplication app)
        {
            app.MapGet("/questionnaire/complete", async (DiagErrorDb db) =>
            {
                return await db.Questionnaires.Include(q => q.Questions).ThenInclude(q => q.Answers).ToListAsync();
            }).WithOpenApi(operation => new(operation)
            {
                Summary = "Get all questionnaires",
                Description = "This endpoint retrieves all questionnaires with their associated questions and stored answers."
            }).WithTags("Obsolete");

            app.MapGet("/questionnaire/complete/filter", async (DiagErrorDb db, int? page, int? pageSize, string? id, string? identifier, string? language, int? lastDays) =>
            {
                // Calculate the number of items to skip and take based on the page and pageSize parameters
                int skip = (page.GetValueOrDefault(1) - 1) * pageSize.GetValueOrDefault(10);
                int take = pageSize.GetValueOrDefault(10);

                //Retrieving all stored questionnaires with questions and answers
                IQueryable<Questionnaire> questionnaires;

                //lastDays was provided, filter out older answers
                if(lastDays != null)
                {
                    //Date "lastDays" ago
                    var filterDate = DateOnly.FromDateTime(DateTime.Now.AddDays((double)lastDays * -1 ));

                    questionnaires = db.Questionnaires
                        .Include(q => q.Questions)
                            .ThenInclude(q => q.Answers.Where(a => a.Date >= filterDate))
                        .Include(q => q.Questions)
                            .ThenInclude(q => q.Options)
                        .AsQueryable();
                }
                //lastDays was not provided, filter nothing out
                else
                {
                    questionnaires = db.Questionnaires
                            .Include(q => q.Questions)
                                .ThenInclude(q => q.Answers)
                            .Include(q => q.Questions)
                                .ThenInclude(q => q.Options)
                            .AsQueryable();
                }

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
                var pageCount = Math.Ceiling((double)questionnaires.Count() / (double)take);
                questionnaires = questionnaires.Skip(skip).Take(take);
                var data = await questionnaires.ToListAsync();

                return Results.Ok(new { pageCount, data });

            }).WithOpenApi(operation => new(operation)
            {
                Summary = "Get all questionnaires with filtering",
                Description = "This endpoint retrieves all questionnaires with their associated questions and stored answers. You can filter the list with id, identifier and language of the wished questionnaire. The list is paginated with page and pageSize." +
                "<br><br>" + //two breaks included in the text
                "e.g. There are 10 questionnaires, but my page can only handle 3 at once. If the endpoint es called from page number 2, there will be the second three questionnaires returned."
            }).WithTags("Questionnaire-Complete");


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
                var pageCount = Math.Ceiling((double)questionnaires.Count() / (double)take);

                questionnaires = questionnaires.Skip(skip).Take(take);

                var data = await questionnaires.ToListAsync();

                return Results.Ok(new { pageCount, data });
            }).WithOpenApi(operation => new(operation)
            {
                Summary = "Get all questionnaires with filtering",
                Description = "This endpoint retrieves all questionnaires with their associated questions. You can filter the list with id, identifier and language of the wished questionnaire. The list is paginated with page and pageSize." +
                "<br><br>" + //two breaks included in the text
                "e.g. There are 10 questionnaires, but my page can only handle 3 at once. If the endpoint es called from page number 2, there will be the second three questionnaires returned."
            }).WithTags("Questionnaire-Light");

            app.MapPost("/questionnaire/light", async (DiagErrorDb db, Questionnaire[] questionnaires) =>
            {
                try {
                    await db.Questionnaires.AddRangeAsync(questionnaires);
                    await db.SaveChangesAsync();
                    return Results.Ok();
                }
                catch (Exception e) {
                        return Results.Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: $"Exception Message: '{e.Message}'");
                }
            }).WithTags("Questionnaire-Light");

            app.MapGet("/questionnaire/file/create", async (DiagErrorDb db, string? identifier, string? language) => 
            {
                try {
                    if(string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(language))
                    {
                        return Results.BadRequest();
                    }

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

                    var questionnaires = db.Questionnaires
                            .Include(q => q.Questions)
                                .ThenInclude(q => q.Answers)
                            .Include(q => q.Questions)
                                .ThenInclude(q => q.Options)
                            .AsQueryable();

                    var data = await questionnaires
                        .Where(q => q.Identifier == identifier)
                        .Where(q => q.Language == Enum.Parse<Language>(language))
                        .ToListAsync();

                    //do not excape unicode characters such as öäü
                    JsonSerializerOptions jso = new JsonSerializerOptions();
                    jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                    string jsonString = JsonSerializer.Serialize(data, jso);

                    //path variables
                    char seperator = System.IO.Path.DirectorySeparatorChar;
                    string filename = $"{identifier}[{language}][answers-all][{DateTime.Now.ToString("yyyy-MM-dd")}].json";
                    string currentDir = System.IO.Directory.GetCurrentDirectory();

                    string fileName = Path.Combine(currentDir, "fileOut", filename);
                    
                    await File.WriteAllTextAsync(fileName, jsonString);
                    return Results.Ok(new {fileName});
                }
                catch (Exception e) {
                        return Results.Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: $"Exception Message: '{e.Message}'");
                }
            }).WithOpenApi(operation => new(operation)
            {
                Summary = "Create a Questionnaire JSON",
                Description = "Creates a Questionnaire JSON on the filesystem of the server running the api. Returns the created file's path"
            }).WithTags("Questionnaire-Complete");
        }
    }
}
