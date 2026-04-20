using Domain.Aggregates.Workouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Workouts.Abstractions;

public interface IWorkoutService
{
    Task<IReadOnlyList<Workout>> GetWorkoutsAsync(CancellationToken ct = default);

    Task<Workout?> GetWorkoutByIdAsync(string id, CancellationToken ct = default);
}
