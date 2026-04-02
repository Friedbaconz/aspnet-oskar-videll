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
        public async Task<IReadOnlyList<Booking>> GetBookingsAsync(CancellationToken ct = default)
        {
            var bookings = await repo.GetAllAsync(ct);
            return bookings;
        }
    }
}
