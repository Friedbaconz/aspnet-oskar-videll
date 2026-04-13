using Infrastructure.Persistence.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Intergration
{
    internal sealed class SqliteInMemoryFixture : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<CoreFitnessDbContext> _options;
        private bool _disposed;

        public SqliteInMemoryFixture()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<CoreFitnessDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Ensure database schema is created
            using var context = CreateContext();
            context.Database.EnsureCreated();
        }

        public CoreFitnessDbContext CreateContext()
        {
            return (CoreFitnessDbContext)Activator.CreateInstance(typeof(CoreFitnessDbContext), new object[] { _options })!;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _connection?.Close();
            _connection?.Dispose();
            _disposed = true;
        }
    }
}
