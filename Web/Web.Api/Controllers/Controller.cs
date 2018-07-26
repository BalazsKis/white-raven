using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Shared.Basics;

namespace WhiteRaven.Web.Api.Controllers
{
    public abstract class Controller : ControllerBase
    {
        protected string GetCurrentUserEmailAddress()
        {
            var email = HttpContext.User?.Claims?.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email.IsBlank())
                throw new UnauthorizedAccessException("The user's email address was not included in the token");

            return email;
        }

        protected UserLevel GetCurrentUserLevel(HttpContext context)
        {
            var roleFromClaim = context.User?.Claims?.SingleOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (roleFromClaim == default(string))
                throw new Exception("Could not read user role from the provided token");

            var levelParsed = int.TryParse(roleFromClaim, out var level);

            if (!levelParsed)
                throw new Exception("Could not parse user role from the provided token");

            return (UserLevel)level;
        }
    }
}