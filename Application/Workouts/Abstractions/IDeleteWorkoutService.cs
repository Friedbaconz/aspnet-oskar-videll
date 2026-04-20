using Application.Common.Results;

namespace Application.Workouts.Abstractions
{
    public interface IDeleteWorkoutService
    {
        Task<Result> ExecuteAsync(string id, CancellationToken ct = default);
    }
}