using Application.Bookings.Services;
using Domain.Abstractions.Repositories.Bookings;
using Domain.Aggregates.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Bookings
{
    public sealed class BookingService(IBookingRepository repo) : IBookingService
    {
        public Task<IReadOnlyList<Booking>> GetBookingsAsync(CancellationToken ct = default)
        {
            var bookings = repo.GetAllAsync(ct);
            return bookings;
        }
    }
}
