using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class FirebaseAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public FirebaseAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string authToken = context.Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authToken) || !authToken.StartsWith("Bearer "))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        string idToken = authToken.Substring("Bearer ".Length);

        try
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            // Authentication successful, proceed with the request
            var claims = decodedToken.Claims.Select(x => new Claim(x.Key, x.Value.ToString()));
            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Firebase"));
            await _next(context);
        }
        catch (FirebaseAuthException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}