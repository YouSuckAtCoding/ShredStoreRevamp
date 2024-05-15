using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ShredStore.Jwt
{
    public static class JwtStartupConfig
    {
        private const string ClaimExpectedValue = "true";
        private const string ConfigJwtKeyPath = "Jwt:Key";
        private const string ConfigJwtIssuerPath = "Jwt:Issuer";
        private const string ConfigJwtAudiencePath = "Jwt:Audience";
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config[ConfigJwtKeyPath]!)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = config[ConfigJwtIssuerPath],
                    ValidAudience = config[ConfigJwtAudiencePath],
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });

            services.AddAuthorization(x =>
            {
                x.AddPolicy(AuthConstants.AdminUserPolicyName, p => p.RequireClaim(AuthConstants.AdminUserClaimName, ClaimExpectedValue));

                x.AddPolicy(AuthConstants.CustomerPolicyName, p => p.RequireAssertion(c =>
                c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: ClaimExpectedValue }) ||
                c.User.HasClaim(m => m is { Type: AuthConstants.CustomerClaimName, Value: ClaimExpectedValue }) ||
                c.User.HasClaim(m => m is { Type: AuthConstants.ShopClaimName, Value: ClaimExpectedValue })));

                x.AddPolicy(AuthConstants.ShopPolicyName, p => p.RequireAssertion(c =>
                c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: ClaimExpectedValue }) ||
                c.User.HasClaim(m => m is { Type: AuthConstants.ShopClaimName, Value: ClaimExpectedValue })));
            });

            return services;
        }


       
    }
}
