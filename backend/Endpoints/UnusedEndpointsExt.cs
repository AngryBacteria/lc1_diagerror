using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.EndpointFilters;

namespace backend.Endpoints
{
    public static class UnusedEndpointsExt
    {
        public static void MapUnusedEndpoints(this WebApplication app)
        {
            // ANSWER ENDPOINTS
            app.MapGet("/api/answer/filter", async (DiagErrorDb db, string? questionnaireIdentifier) =>
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
            }).WithTags("Obsolete").AddEndpointFilter<FirebaseAuthFilter>();


            // QUESTION ENDPOINTS
            app.MapGet("/api/question/complete", async (DiagErrorDb db) =>
            {
                return await db.Questions.Include(a => a.Answers).ToListAsync();
            }).WithTags("Obsolete").AddEndpointFilter<FirebaseAuthFilter>();

            app.MapGet("/api/question/light", async (DiagErrorDb db) =>
            {
                return await db.Questions.ToListAsync();
            }).WithTags("Obsolete").AddEndpointFilter<FirebaseAuthFilter>();

            app.MapPost("/api/question/light", async (DiagErrorDb db, Question question) =>
            {
                await db.Questions.AddAsync(question);
                await db.SaveChangesAsync();
                return Results.Created($"/question/light/{question.QuestionId}", question);
            }).WithTags("Obsolete").AddEndpointFilter<FirebaseAuthFilter>();

            // QUESTIONNAIRE ENDPOINTS
            app.MapGet("/api/questionnaire/complete", async (DiagErrorDb db) =>
            {
                return await db.Questionnaires
                    .Include(questionnaire => questionnaire.Questions)
                        .ThenInclude(question => question.Answers)
                    .Include(questionnaire => questionnaire.Questions)
                        .ThenInclude(question => question.Options)
                    .ToListAsync();
            }).WithOpenApi(operation => new(operation)
            {
                Summary = "Get all questionnaires",
                Description = "This endpoint retrieves all questionnaires with their associated questions and stored answers."
            }).WithTags("Obsolete").AddEndpointFilter<FirebaseAuthFilter>();

            app.MapGet("/api/questionnaire/light", async (DiagErrorDb db) =>
            {
                return await db.Questionnaires
                    .Include(questionnaire => questionnaire.Questions)
                    .ThenInclude(question => question.Options)
                    .ToListAsync();
            }).WithOpenApi(operation => new(operation)
            {
                Summary = "Get all questionnaires without answers",
                Description = "This endpoint retrieves all questionnaires with their associated questions but without the stored answers."
            }).WithTags("Obsolete").AddEndpointFilter<FirebaseAuthFilter>();
        }
    }
}
