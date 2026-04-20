using Application.Common.Results;
using Application.Workouts.Inputs;
using Domain.Aggregates.Workouts;

namespace Application.Workouts.Abstractions
{
    public interface IUpdateWorkoutService
    {
        Task<Result<Workout>> ExecuteAsync(UpdateWorkoutInput input, CancellationToken ct = default);
    }
}