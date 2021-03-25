namespace Zoo.Api.Tests.EndToEnd.Core
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Policy;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;

    internal class FakePolicyEvaluator : PolicyEvaluator
    {
        private readonly (SigningCredentials SigningCredentials, SymmetricSecurityKey symmetricSecurityKey, string
            Issuer, string Audience) tokenConfiguration;

        public FakePolicyEvaluator(
            IAuthorizationService authorizationService,
            (SigningCredentials SigningCredentials, SymmetricSecurityKey symmetricSecurityKey, string Issuer, string
                Audience) tokenConfiguration)
            : base(authorizationService)
        {
            this.tokenConfiguration = tokenConfiguration;
        }

        public override async Task<AuthenticateResult> AuthenticateAsync(
            AuthorizationPolicy policy,
            HttpContext context)
        {
            var result = await base.AuthenticateAsync(policy, context);
            if (result.Succeeded)
                return result;

            var authorization = context.Request.Headers["Authorization"].ToString();
            var token = authorization.Replace($"{JwtBearerDefaults.AuthenticationScheme} ", string.Empty);
            context.User = this.ValidateToken(token);
            return AuthenticateResult.Success(new AuthenticationTicket(context.User, "context.User"));
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            var validationParameters = new TokenValidationParameters
                                           {
                                               RoleClaimType = ClaimTypes.Role,
                                               ValidateAudience = true,
                                               ValidateIssuer = true,
                                               ValidateLifetime = true,
                                               RequireSignedTokens = true,
                                               NameClaimType = ClaimTypes.NameIdentifier,
                                               ValidAudience = this.tokenConfiguration.Audience,
                                               ValidIssuer = this.tokenConfiguration.Issuer,
                                               IssuerSigningKey = this.tokenConfiguration
                                                                      .symmetricSecurityKey
                                           };
            var principal = new JwtSecurityTokenHandler().ValidateToken(
                jwtToken,
                validationParameters,
                out _);
            return principal;
        }
    }
}