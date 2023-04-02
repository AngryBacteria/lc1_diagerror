using FirebaseAdmin.Auth;
namespace backend.EndpointFilters
{
    public class FirebaseAuthFilter : IEndpointFilter
    {

        private readonly FirebaseAuth _firebaseAuth;

        //Constructor
        public FirebaseAuthFilter(FirebaseAuth firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;
            Console.WriteLine(_firebaseAuth);
        }

        //Filter
        public virtual async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            Console.WriteLine("Code runs before route");

            if (context.HttpContext.Request.Headers.ContainsKey("Authorization")) {
                var result = await next(context);
                Console.WriteLine("Code runs after route");
                return result;
            }
            else {
                return Results.Problem("You are not authrized for this route");
            }            
        }
    }
}
