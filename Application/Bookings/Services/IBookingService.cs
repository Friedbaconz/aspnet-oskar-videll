using System;
using System.Collections.Generic;
using System.Text;
using Domain.Aggregates.Bookings;

namespace Application.Bookings.Services
{
    public interface IBookingService
    {
        Task<IReadOnlyList<Booking>> GetBookingsAsync(CancellationToken ct = default);
    }
}
