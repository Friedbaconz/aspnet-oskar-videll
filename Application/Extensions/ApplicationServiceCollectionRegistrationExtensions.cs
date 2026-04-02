using Application.Bookings;
using Application.Bookings.Services;
using Application.Memberships;
using Application.Memberships.Services;
using Application.Workouts;
using Application.Workouts.Services;
using Domain.Aggregates.Workouts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Application.Extensions;

public static class ApplicationServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IBenefitService, BenefitService>();
        services.AddUserSevice();
        return services;
    }
}