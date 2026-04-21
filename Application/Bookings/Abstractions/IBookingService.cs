using System;
using System.Collections.Generic;
using System.Text;
using Domain.Aggregates.Bookings;

namespace Application.Bookings.Abstractions
{
    public interface IBookingService
    {
        Task<IReadOnlyList<Booking>> GetBookingsAsync(CancellationToken ct = default);

        Task<Booking?> GetBookingByIdAsync(string id, CancellationToken ct = default);

        Task<IReadOnlyList<Booking?>> GetAllBookingByUserIdAsync(string id, CancellationToken ct = default);
    }
}
