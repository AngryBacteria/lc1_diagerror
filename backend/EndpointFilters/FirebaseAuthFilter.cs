using FirebaseAdmin.Auth;
namespace backend.EndpointFilters
{
    /// <summary>
    /// Filter to secure endpoints with authentication and authorization 
    /// </summary>
    public class FirebaseAuthFilter : IEndpointFilter
    {

        private readonly FirebaseAuth _firebaseAuth;

        /// <summary>
        /// Constructor for the Endpointfilter
        /// </summary>
        /// <param name="firebaseAuth">Firebase auth instance</param>
        public FirebaseAuthFilter(FirebaseAuth firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;
        }

        //Filter
        /// <summary>
        /// Endpoint filter function. Checks if the firebase token is present in the "Authorization" header.
        /// Additionally it checks if the token has the admin claim set to true
        /// </summary>
        /// <param name="context">context passed from the http request</param>
        /// <param name="next">function to call after the filter</param>
        /// <returns></returns>
        public virtual async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            //Firebase token verification throws errors. They need to be catched
            try {
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
                FirebaseToken firebaseToken = await _firebaseAuth.VerifyIdTokenAsync(authToken);
                
                //If firebaseToken has the admin claim and its true, authorize 
                if (firebaseToken.Claims.TryGetValue("admin", out var isAdmin))
                    {
                        if ((bool)isAdmin)
                        {
                            var result = await next(context);
                            return result;
                        }
                        return Results.Unauthorized();
                    }
                
                else {
                    return Results.Unauthorized(); 
                }
            }
            catch(Exception) {
                return Results.Unauthorized();
            }
        }
    }
}
