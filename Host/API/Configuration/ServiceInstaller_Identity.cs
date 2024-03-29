﻿namespace API.Configuration;

using Domain.Attributes;
using Domain.Configuration;
using Domain.Entities.Identity;
using Domain.Extensions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[ConfigOrder(0)]
public class ServiceInstaller_Identity : IServiceInstaller
{
    public void Configure(IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = bool.TryParse(configuration.GetFromEnvironmentVariable("IDT", "SIGNIN", "RQR_CONF_PHONE_NUM"), out var signinRqrConfPhoneNum) && signinRqrConfPhoneNum;
                options.SignIn.RequireConfirmedEmail = !bool.TryParse(configuration.GetFromEnvironmentVariable("IDT", "SIGNIN", "RQR_CONF_EMAIL"), out var signinRqrConfEmail) || signinRqrConfEmail;
                options.SignIn.RequireConfirmedAccount = !bool.TryParse(configuration.GetFromEnvironmentVariable("IDT", "SIGNIN", "RQR_CONF_ACCOUNT"), out var signinRqrConfAccount) || signinRqrConfAccount;

                options.Password.RequiredLength = int.TryParse(configuration.GetFromEnvironmentVariable("IDT", "PWD", "LENGTH"), out var pwdLength) ? pwdLength : 8;
                options.Password.RequireDigit = !bool.TryParse(configuration.GetFromEnvironmentVariable("IDT", "PWD", "RQR_DIGIT"), out var pwdRqrDigit) || pwdRqrDigit;
                options.Password.RequireLowercase = !bool.TryParse(configuration.GetFromEnvironmentVariable("IDT", "PWD", "RQR_LWR_CSE"), out var pwdRqrLwrCse) || pwdRqrLwrCse;
                options.Password.RequireUppercase = !bool.TryParse(configuration.GetFromEnvironmentVariable("IDT", "PWD", "RQR_UPR_CSE"), out var pwdRqrUprCse) || pwdRqrUprCse;
                options.Password.RequireNonAlphanumeric = bool.TryParse(configuration.GetFromEnvironmentVariable("IDT", "PWD", "RQR_NO_ALPHANUM"), out var pwdRqrNoAlphaNum) && pwdRqrNoAlphaNum;

                options.Lockout.AllowedForNewUsers = !bool.TryParse(configuration.GetFromEnvironmentVariable("IDT", "LCK", "ALLOW_NEW_USR"), out var lckAllowNewUsr) || lckAllowNewUsr;
                options.Lockout.MaxFailedAccessAttempts = int.TryParse(configuration.GetFromEnvironmentVariable("IDT", "LCK", "MAX_ATTEMPS"), out var lckMaxAttemps) ? lckMaxAttemps : 5;
                options.Lockout.DefaultLockoutTimeSpan = int.TryParse(configuration.GetFromEnvironmentVariable("IDT", "LCK", "TIME"), out var lckTime) ? TimeSpan.FromMinutes(lckTime) : TimeSpan.FromMinutes(5);

                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();
    }

    public void Install(WebApplication applicationBuilder)
    {
    }
}
