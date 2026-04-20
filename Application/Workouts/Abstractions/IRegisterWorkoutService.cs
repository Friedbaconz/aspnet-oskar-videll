using Application.Common.Results;
using Application.Workouts.Inputs;

namespace Application.Workouts.Abstractions
{
    public interface IRegisterWorkoutService
    {
        Task<Result<string?>> ExecuteAsync(RegisterWorkoutInput input, CancellationToken ct = default);
    }
}