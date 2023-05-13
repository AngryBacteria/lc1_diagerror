using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints
{
    public static class AnswerEndpointsExt
    {
        public static void MapAnswerEndpoints(this WebApplication app)
        {
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
