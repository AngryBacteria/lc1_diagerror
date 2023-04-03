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
        }

        //Filter
        public virtual async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            //Firebase token verification throws errors. They need to be catched
            try {
                Console.WriteLine("Code runs before route");

                //No Token
                if(!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader)){
                    return Results.Unauthorized();
                }

                string authHeaderValue = authorizationHeader.ToString();

                //Token has wrong format
                if (string.IsNullOrEmpty(authHeaderValue) || !authHeaderValue.StartsWith("Bearer ")) {
                    return Results.Unauthorized();
                }

                String authToken = authHeaderValue.Substring("Bearer ".Length).Trim();
                Console.WriteLine(authToken);

                FirebaseToken firebaseToken = await _firebaseAuth.VerifyIdTokenAsync(authToken);
                
                //If firebaseToken has the admin claim and its true, authorize 
                if (firebaseToken.Claims.TryGetValue("admin", out var isAdmin))
                    {
                        if ((bool)isAdmin)
                        {
                            var result = await next(context);
                            Console.WriteLine("Code runs after route");
                            return result;
                        }
                        return Results.Unauthorized();
                    }
                
                else {
                    return Results.Unauthorized(); 
                }
            }
            catch(Exception e) {
                return Results.Unauthorized();
            }
        }
    }
}
