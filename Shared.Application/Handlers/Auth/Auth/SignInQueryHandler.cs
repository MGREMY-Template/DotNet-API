namespace Shared.Application.Handlers.Auth.Auth;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Shared.Core.DataTransferObject;
using Shared.Core.DataTransferObject.Auth.AuthController.Output;
using Shared.Core.Entities.Identity;
using Shared.Core.Extensions;
using Shared.Core.Queries.Auth.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class SignInQueryHandler : IRequestHandler<SignInQuery, Result<SignInOutput>>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer _stringLocalizer;

    public SignInQueryHandler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        IStringLocalizer<Core.Resources.Application.Services.Auth.AuthService> stringLocalizer)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._configuration = configuration;
        this._stringLocalizer = stringLocalizer;
    }

    public async Task<Result<SignInOutput>> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        User user = await this._userManager.FindByEmailAsync(request.Input.Email);

        return Result.Create(user, 200, 404, this._stringLocalizer.GetString(Core.Resources.Application.Services.Auth.AuthServiceConstants.UserNotFound))
            .EnsureAsync(this._signInManager.CanSignInAsync, 400, this._stringLocalizer.GetString(Core.Resources.Application.Services.Auth.AuthServiceConstants.UserSignInError))
            .EnsureAsync(async x => await this._userManager.CheckPasswordAsync(x, request.Input.Password), 400, this._stringLocalizer.GetString(Core.Resources.Application.Services.Auth.AuthServiceConstants.PasswordIncorrect))
            .MapAsync(async x =>
            {
                _ = this._configuration.AsEnumerable();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetFromEnvironmentVariable("JWT", "KEY")));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                System.Collections.Generic.IList<System.Security.Claims.Claim> claims = await this._userManager.GetClaimsAsync(x);

                var tokenOptions = new JwtSecurityToken(this._configuration.GetFromEnvironmentVariable("JWT", "ISSUER"),
                    this._configuration.GetFromEnvironmentVariable("JWT", "AUDIENCE"),
                    claims,
                    expires: DateTime.Now.AddMinutes(3600),
                    signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return new SignInOutput
                {
                    Token = token,
                };
            });
    }
}
