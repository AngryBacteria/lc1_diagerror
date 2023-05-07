using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints
{
    public static class QuestionEndpointsExt
    {
        public static void MapQuestionEndpoints(this WebApplication app)
        {
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
        }
    }
}
