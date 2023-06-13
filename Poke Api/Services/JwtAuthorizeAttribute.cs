using Microsoft.AspNetCore.Mvc;

public class JwtAuthorizeAttribute : TypeFilterAttribute
{
    public JwtAuthorizeAttribute() : base(typeof(JwtAuthorizationFilter))
    {
    }
}
