using Application.Bookings.Abstractions;
using Application.Bookings.Services;
using Application.Memberships.Abstractions;
using Application.Memberships.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions
{
    public static class BookingRegistrationExtension
    {
        public static IServiceCollection AddBookingService(this IServiceCollection services)
        {
            services.AddScoped<IUpdateBookingService, UpdateBookingService>();
            services.AddScoped<IRegisterBookingService, RegisterBookingService>();
            services.AddScoped<IDeleteBookingService, DeleteBookingService>();

            return services;
        }
    }
}
