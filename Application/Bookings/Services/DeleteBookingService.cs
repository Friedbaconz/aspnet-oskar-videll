using Application.Bookings.Abstractions;
using Application.Common.Results;
using Domain.Abstractions.Repositories.Bookings;
using Domain.Abstractions.Repositories.Users;
using Domain.Abstractions.Repositories.Workouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Bookings.Services;

public sealed class DeleteBookingService(IBookingRepository repo, IWorkoutRepository workoutrepo, IUserRepository userrepo) : IDeleteBookingService
{
    public async Task<Result> ExecuteAsync(string id, CancellationToken ct = default)
    {
        if(id == null) throw new ArgumentNullException("id is null");

        var result = await repo.GetByIdAsync(id, ct);

        if (result == null)
        {
            throw new Exception("no membership by that id");
        }

        var workout = await workoutrepo.GetByIdAsync(result.WorkoutId, ct);

        if (workout == null)
        {
            throw new ApplicationException("Workout not found");
        }

        workout.Users.ToList().Remove(result.UserId);

        var workoutresult = await workoutrepo.UpdateAsync(workout, ct);

        if (!workoutresult)
        {
            throw new ApplicationException("Failed to update workout with new booking");
        }

        var user = await userrepo.GetUserByUserIdAsync(result.UserId, ct);

        if (user == null)
        {
            throw new ApplicationException("User not found");
        }

        user.WorkoutsId.ToList().Remove(result.WorkoutId);

        var userresult = await userrepo.UpdateAsync(user, ct);

        if (!userresult)
        {
            throw new ApplicationException("Failed to update user with new booking");
        }

        var success = await repo.RemoveAsync(result, ct);

        return success
                ? Result.Ok()
                : Result.Error("Failed to delete user profile");
    }
}
