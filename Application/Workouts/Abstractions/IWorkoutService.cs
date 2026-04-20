using Domain.Aggregates.Workouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Workouts.Abstractions;

public interface IWorkoutService
{
    Task<IReadOnlyList<Workout>> GetWorkoutsAsync(CancellationToken ct = default);
}
