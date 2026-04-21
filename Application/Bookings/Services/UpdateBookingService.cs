using Application.Bookings.Abstractions;
using Application.Bookings.Inputs;
using Application.Common.Results;
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

public sealed class UpdateBookingService(IBookingRepository repo, IWorkoutRepository workoutrepo, IUserRepository userrepo) : IUpdateBookingService
{
    public async Task<Result<Booking>> ExecuteAsync(UpdateBookingInput input, CancellationToken ct = default)
    {
        try
        {
            var booking = await repo.GetByIdAsync(input.Id, ct);
            if (booking is null)
            {
                return Result<Booking>.NotFound($"Booking with ID {input.Id} not found.");
            }

            booking.Update(input.Id, input.WorkoutId, input.UserId);
            await repo.UpdateAsync(booking, ct);
            return Result<Booking>.Ok(booking);
        }
        catch (Exception ex)
        {
            return Result<Booking>.Error($"An error occurred while updating the booking: {ex.Message}");
        }
    }
}
