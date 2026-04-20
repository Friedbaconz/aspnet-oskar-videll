using Application.Memberships.Abstractions;
using Application.Memberships.Services;
using Application.Workouts.Abstractions;
using Application.Workouts.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions;

public static class MembershipRegistrationExtension
{
    public static IServiceCollection AddMemberShipService(this IServiceCollection services)
    {
        services.AddScoped<IUpdateMembershipService, UpdateMembershipService>();
        services.AddScoped<IRegisterMembershipService, RegisterMembershipService>();
        services.AddScoped<IDeleteMembershipService, DeleteMembershipService>();

        return services;
    }
}
