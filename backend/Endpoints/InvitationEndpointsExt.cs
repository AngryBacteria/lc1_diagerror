using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints
{
    public static class InvitationEndpointsExt
    {
        public static void MapInvitationEndpoints(this WebApplication app)
        {
            //////// Invitation ////////
            //Endpoints for POST Invitation code
            app.MapPost("/invitation", async (DiagErrorDb db, string invitationCode, string? language) =>
            {
                try
                {
                    if (invitationCode.Length < 2)
                    {
                        throw new ArgumentException("The identifier is too short. It has to consist of 2 digits representing a questionnaire identifier and 6 digits representing unique invitation");
                    };

                    //TODO check if answer exists with that invitationCode

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
                    Boolean? exactMatch = null;
                    IEnumerable<Questionnaire>? result = null;
                    if(filteredQuestionnaires.Any()) {
                        result = filteredQuestionnaires;
                        exactMatch = true;
                    }
                    else {
                        result = questionnaires;
                        exactMatch = false;
                    }
                    return (bool)exactMatch ? Results.Ok(result) : Results.Accepted(null, result);
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
                Summary = "Retrieve questionnaires by invitation code",
                Description = "This endpoint validates an invitation code and returns the matching questionnaires. If a language parameter is provided, the endpoint additionally returns only questionnaires that match the specified language."
            }).WithTags("Invitation");
        }
    }
}
