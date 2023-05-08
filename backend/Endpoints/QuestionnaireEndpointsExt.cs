using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints
{
    public static class QuestionnaireEndpointsExt
    {
        public static void MapQuestionnaireEndpoints(this WebApplication app)
        {
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

            //Endpoints for POST Questionnaires without answers
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
        }
    }
}
