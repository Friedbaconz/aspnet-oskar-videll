using Application.Bookings;
using Application.Memberships;
using Application.Memberships.Services;
using Application.Workouts;
using Domain.Aggregates.Workouts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Memberships.Abstractions;
using Application.Workouts.Abstractions;
using Application.Bookings.Abstractions;
namespace Application.Extensions;

public static class ApplicationServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IBenefitService, BenefitService>();


        services.AddBookingService();
        services.AddMemberShipService();
        services.AddWorkoutService();
        services.AddUserSevice();
        return services;
    }
}