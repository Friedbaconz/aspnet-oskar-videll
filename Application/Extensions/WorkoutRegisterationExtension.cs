using Application.Users.Abstractions;
using Application.Users.Services;
using Application.Workouts.Abstractions;
using Application.Workouts.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions;

public static class WorkoutRegisterationExtension
{
    public static IServiceCollection AddWorkoutService(this IServiceCollection services)
    {
        services.AddScoped<IRegisterWorkoutService, RegisterWorkoutService>();
        services.AddScoped<IUpdateWorkoutService, UpdateWorkoutService>();
        services.AddScoped<IDeleteWorkoutService, DeleteWorkoutService>();

        return services;
    }
}
