using Application.Bookings.Inputs;
using Application.Common.Results;

namespace Application.Bookings.Abstractions
{
    public interface IRegisterBookingService
    {
        Task<Result<string?>> ExecuteAsync(RegisterBookingInput input, CancellationToken ct = default);
    }
}