using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Yashil.WebHost
{
    public class YashilKeyJwtValidationHandler : JwtSecurityTokenHandler
    {
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            //We can read the token before we've begun validating it.
            //And let the framework take it from here.
            try
            {
                var claimsPrincipal = base.ValidateToken(token, validationParameters, out validatedToken);
                return claimsPrincipal;
            }
            catch (Exception)
            {
                validatedToken = null;
                return new ClaimsPrincipal();
            }
        }
    }
}
