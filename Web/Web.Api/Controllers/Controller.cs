using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Shared.Basics;

namespace WhiteRaven.Web.Api.Controllers
{
    /// <summary>
    /// Abstract base class 
    /// </summary>
    public abstract class Controller : ControllerBase
    {
        /// <summary>
        /// Returns the current user's email address using the HTTP Context, and the provided token
        /// </summary>
        /// <returns>The current user's email address</returns>
        /// <exception cref="UnauthorizedAccessException">If the user's email address was not included in the token</exception>
        protected string GetCurrentUserEmailAddress()
        {
            var email = HttpContext.User?.Claims?.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email.IsBlank())
                throw new UnauthorizedAccessException("The user's email address was not included in the token");

            return email;
        }

        /// <summary>
        /// Returns the current user's access level using the HTTP Context, and the provided token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">
        /// If the user's access level was not included in the token
        /// or
        /// If the user's access level couldn't be parsed
        /// </exception>
        protected UserLevel GetCurrentUserLevel()
        {
            var roleFromClaim = HttpContext.User?.Claims?.SingleOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (roleFromClaim == default(string))
                throw new UnauthorizedAccessException("Could not read user role from the provided token");

            var levelParsed = int.TryParse(roleFromClaim, out var level);

            if (!levelParsed)
                throw new UnauthorizedAccessException("Could not parse user role from the provided token");

            return (UserLevel)level;
        }
    }
}