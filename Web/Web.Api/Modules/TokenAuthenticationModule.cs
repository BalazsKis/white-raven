using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WhiteRaven.Shared.DependencyInjection;

namespace WhiteRaven.Web.Api.Modules
{
    /// <summary>
    /// Contains registrations and configurations for the token-based authentication
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    public class TokenAuthenticationModule : ModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenAuthenticationModule"/> class.
        /// </summary>
        public TokenAuthenticationModule(IConfiguration configuration) : base(configuration)
        {
        }


        /// <summary>
        /// Configures and registers token properties
        /// </summary>
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
        }
    }
}