using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Poke_Api.Services;
using System.Security.Claims;

public class JwtAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authenticationService = context.HttpContext.RequestServices.GetService<AuthenticationService>();

        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


        if (token != null)
        {
            try
            {
              
                var userClaims = authenticationService.ValidateToken(token);
                if (!authenticationService.ValidateLevel(context.HttpContext.Request, userClaims))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(userClaims));

                return; 
            }
            catch
            {
               
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        context.Result = new UnauthorizedResult();
    }
}
