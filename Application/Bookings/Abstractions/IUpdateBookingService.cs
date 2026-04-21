using Application.Bookings.Inputs;
using Application.Common.Results;
using Application.Workouts.Inputs;
using Domain.Aggregates.Bookings;

namespace Application.Bookings.Abstractions
{
    public interface IUpdateBookingService
    {
        Task<Result<Booking>> ExecuteAsync(UpdateBookingInput input, CancellationToken ct = default);
    }
}