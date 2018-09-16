using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Shared.Library.Configuration;
using WhiteRaven.Web.Api.Configurations;

namespace WhiteRaven.Web.Api.Modules
{
    /// <summary>
    /// Contains registrations and configurations for the token-based authentication
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    [ForEnvironment(Environment.Development, Environment.Staging, Environment.Production)]
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
            var tokenParams = Configuration.GetSection("Jwt").Get<TokenGenerationParameters>();

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
                        ValidIssuer = tokenParams.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenParams.Key))
                    };
                });
        }
    }
}