using Application.Workouts.Inputs;
using Application.Workouts.Services;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Workouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Intergration.Infrastructure
{
    public class WorkoutApplicationTest
    {
        [Fact]
        public async Task RegisterWorkoutService_CreatesWorkout_And_AddsToRepository()
        {
            var repo = new InMemoryWorkoutRepository();
            var service = new RegisterWorkoutService(repo);

            var input = new RegisterWorkoutInput(
                Name: "Morning Yoga",
                Category: "Yoga",
                Instructions: "Bring a mat",
                Date: new DateTime(2026, 4, 27),
                Time: new TimeSpan(1, 0, 0)
            );

            var result = await service.ExecuteAsync(input, CancellationToken.None);

            Assert.True(result.Success);
            Assert.False(string.IsNullOrWhiteSpace(result.Value));

            var stored = await repo.GetByIdAsync(result.Value!, CancellationToken.None);
            Assert.NotNull(stored);
            Assert.Equal(input.Name, stored!.Name);
            Assert.Equal(input.Category, stored.Category);
            Assert.Equal(input.Instructions, stored.Instructions);
            Assert.Equal(input.Date.Date, stored.Date.Date);
            Assert.Equal(input.Time, stored.Time);
        }

        [Fact]
        public async Task UpdateWorkoutService_UpdatesExistingWorkout()
        {
            var repo = new InMemoryWorkoutRepository();

            var id = Guid.NewGuid().ToString();
            var original = Workout.Create(
                id: id,
                name: "Initial",
                category: "Cardio",
                instructions: "Initial instructions",
                date: new DateTime(2026, 4, 20),
                time: new TimeSpan(0, 45, 0),
                users: Enumerable.Empty<string>()
            );

            await repo.AddAsync(original, CancellationToken.None);

            var updateInput = new UpdateWorkoutInput(
                Id: id,
                Name: "Updated Name",
                Category: "Strength",
                Instructions: "Updated instructions",
                Date: new DateTime(2026, 5, 1),
                Time: new TimeSpan(1, 15, 0),
                Users: new List<string> { "user-1", "user-2" }
            );

            var service = new UpdateWorkoutService(repo);

            var result = await service.ExecuteAsync(updateInput, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Value);
            Assert.Equal(updateInput.Name, result.Value!.Name);
            Assert.Equal(updateInput.Category, result.Value.Category);
            Assert.Equal(updateInput.Instructions, result.Value.Instructions);
            Assert.Equal(updateInput.Date.Date, result.Value.Date.Date);
            Assert.Equal(updateInput.Time, result.Value.Time);

            var stored = await repo.GetByIdAsync(id, CancellationToken.None);
            Assert.NotNull(stored);
            Assert.Equal(updateInput.Name, stored!.Name);
            Assert.Equal(updateInput.Category, stored.Category);
        }

        private class InMemoryWorkoutRepository : IWorkoutRepository
        {
            private readonly List<Workout> _store = new();

            public Task AddAsync(Workout model, CancellationToken ct = default)
            {
                _store.Add(model);
                return Task.CompletedTask;
            }

            public Task<IReadOnlyList<Workout>> GetAllAsync(CancellationToken ct = default)
            {
                IReadOnlyList<Workout> list = _store.ToList();
                return Task.FromResult(list);
            }

            public Task<Workout?> GetByIdAsync(string id, CancellationToken ct = default)
            {
                var workout = _store.FirstOrDefault(w => w.Id == id);
                return Task.FromResult(workout);
            }

            public Task<bool> RemoveAsync(Workout model, CancellationToken ct = default)
            {
                var removed = _store.Remove(model);
                return Task.FromResult(removed);
            }

            public Task<bool> UpdateAsync(Workout model, CancellationToken ct = default)
            {
                var existing = _store.FirstOrDefault(w => w.Id == model.Id);
                if (existing is null) return Task.FromResult(false);
                _store.Remove(existing);
                _store.Add(model);
                return Task.FromResult(true);
            }
        }
    }
}
