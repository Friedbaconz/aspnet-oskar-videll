using Application.Common.Results;

namespace Application.Bookings.Abstractions
{
    public interface IDeleteBookingService
    {
        Task<Result> ExecuteAsync(string id, CancellationToken ct = default);
    }
}