using Application.Bookings.Abstractions;
using Domain.Abstractions.Repositories.Bookings;
using Domain.Aggregates.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Bookings;

public sealed class BookingService(IBookingRepository repo) : IBookingService
{
    public async Task<IReadOnlyList<Booking>> GetBookingsAsync(CancellationToken ct = default)
    {
        var bookings = await repo.GetAllAsync(ct);
        return bookings;
    }

    public async Task<Booking?> GetBookingByIdAsync(string id, CancellationToken ct = default)
    {
        var booking = await repo.GetByIdAsync(id, ct);
        return booking;
    }

    public async Task<IReadOnlyList<Booking?>> GetAllBookingByUserIdAsync(string id, CancellationToken ct = default)
    {
        var bookings = await repo.GetAllByUserId(id, ct);
        return bookings;
    }
}
