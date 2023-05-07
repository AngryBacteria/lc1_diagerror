using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints
{
    public static class AnswerEndpointsExt
    {
        public static void MapAnswerEndpoints(this WebApplication app)
        {
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
        }
    }
}
