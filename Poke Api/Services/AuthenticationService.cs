using Microsoft.IdentityModel.Tokens;
using Poke_Api.Models.Rules;
using Poke_Api.Models.User;
using Poke_Api.Repositories.User;
using Poke_Api.Utils;
using System.Collections;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Poke_Api.Services
{
    public class AuthenticationService
    {

        struct Routes
        {
            public List<string> Method;
            public string Path;
            public Routes(List<string> method, string path)
            {
                Method = method;
                Path = path;
            }
        }

        private List<Routes>? _adminRoutes;
        private string? _secretKey;

        public AuthenticationService()
        {
            _adminRoutes = new List<Routes>
            {
                new Routes(new List<string>{ "delete", "put", "get" }, @"^/api/user$" ),
                new Routes(new List<string>{ "delete", "put", "get" }, @"^/api/user/\d+$" ),

            };
        }

        public bool ValidateLevel(HttpRequest request, IEnumerable<Claim> claims)
        {
            try
            {
                foreach (var route in _adminRoutes)
                {
                    if(route.Method.Contains(request.Method.ToLower()) && Regex.IsMatch(request.Path, route.Path))
                    {
                        foreach (var claim in claims)
                        {
                            if (claim.Type == ClaimTypes.Role)
                            {
                                if (claim.Value == "ADMIN")
                                {
                                    return true;
                                }
                            }
                          
                        }
                        return false;
                    }
                }
               
                return true;   
                
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public string Authenticate(UserModel user)
        {
            try
            {
                _secretKey = ENV.SecretKey("PokeApi");
                string token = GenerateToken(user);
                return token;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private Claim[] GenerateClaims(UserModel user)
        {
            try
            {
                Claim[] claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
                foreach (RuleModel rule in user.Rules)
                {
                    Claim[] additionalClaims = new[] { new Claim(ClaimTypes.Role, rule.Name) };
                    claims = claims.Concat(additionalClaims).ToArray();
                }
                return claims;  
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        private string GenerateToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(GenerateClaims(user))
                
               
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<Claim> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            var userClaims = claimsPrincipal.FindAll(c =>c.Type == ClaimTypes.Role || c.Type == ClaimTypes.NameIdentifier || c.Type == ClaimTypes.Email);
            if (userClaims != null && userClaims.Count() > 0)
            {
                return userClaims;
            }

            throw new Exception("Invalid token or missing user ID claim.");
        }

        public int IdByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            var userId = claimsPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (userId != null )
            {
                return int.Parse(userId.Value);
            }

            throw new Exception("Invalid token or missing user ID claim.");
        }

    }
}

