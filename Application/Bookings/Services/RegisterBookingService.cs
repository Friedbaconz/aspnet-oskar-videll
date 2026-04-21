using Application.Bookings.Abstractions;
using Application.Bookings.Inputs;
using Application.Common.Results;
using Application.Workouts.Abstractions;
using Application.Workouts.Inputs;
using Domain.Abstractions.Repositories.Bookings;
using Domain.Abstractions.Repositories.Users;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Bookings;
using Domain.Aggregates.Workouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Bookings.Services;

public sealed class RegisterBookingService(IBookingRepository repo, IWorkoutRepository workoutrepo, IUserRepository userrepo) : IRegisterBookingService
{
    public async Task<Result<string?>> ExecuteAsync(RegisterBookingInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                return Result<string?>.BadRequest("Input must be provided");

            var workout = await workoutrepo.GetByIdAsync(input.workoutId, ct);

            if (workout == null)
            {
                throw new ApplicationException("Workout not found");
            }

            workout.Users.ToList().Add(input.userId);

            var workoutresult = await workoutrepo.UpdateAsync(workout, ct);
            
            if (!workoutresult)
            {
                throw new ApplicationException("Failed to update workout with new booking");
            }

            var user = await userrepo.GetUserByUserIdAsync(input.userId, ct);

            if (user == null) 
            { 
                throw new ApplicationException("User not found");
            }

            user.WorkoutsId.ToList().Add(input.workoutId);

            var userresult = await userrepo.UpdateAsync(user, ct);

            if (!userresult)
            {
                throw new ApplicationException("Failed to update user with new booking");
            }

            var booking = Booking.Create(
                userId: input.userId,
                workoutId: input.workoutId
            );

            await repo.AddAsync(booking, ct);

            return Result<string?>.Ok(booking.Id);
        }
        catch (Exception ex)
        {
            return Result<string?>.Error(ex.Message);
        }
    }
}
