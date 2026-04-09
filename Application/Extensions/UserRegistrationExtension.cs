using Application.Memberships;
using Application.Memberships.Services;
using Application.Users.Abstractions;
using Application.Users.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions;

public static class UserRegistrationExtension
{
    public static IServiceCollection AddUserSevice(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserAccountService, RegisterUserAccountService>();
        services.AddScoped<IGetUserProfileService, GetUserProfileService>();
        services.AddScoped<ISignInUserService, SignInUserService>();
        services.AddScoped<IUpdateUserService, UpdateUserService>();
        services.AddScoped<IRemoveUserService, RemoveUserProfileService>();
        services.AddScoped<IRemoveUserMemembershipService, RemoveUserMemembershipService>();
        return services;
    }
}
