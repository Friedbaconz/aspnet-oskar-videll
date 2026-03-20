using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Contexts.Extensions;

public static class ContextRegistrationExtensions
{
    public static IServiceCollection AddDBContexts(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            Console.WriteLine("Development Environment. Using in-memory SQLite database.");
            services.AddSingleton<SqliteConnection>(_ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:;");
                connection.Open();
                return connection;
            });

            services.AddDbContext<CoreFitnessDbContext>((sp, options) =>
            {
                var connection = sp.GetRequiredService<SqliteConnection>();
                options.UseSqlite(connection);
            });
        }
        else
        {
            Console.WriteLine("Production Environment");

            services.AddDbContext<CoreFitnessDbContext>((sp, options) =>
            {
                var connection = configuration.GetConnectionString("ProductionDatabaseUri")
                    ?? throw new ArgumentException("Connection string for production database is not Provided.");

                options.UseNpgsql(connection);
            });

        }

        return services;
    }
}
