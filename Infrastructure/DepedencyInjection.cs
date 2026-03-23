using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DepedencyInjection
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

    public static IServiceCollection AddPresistance(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddRepositories(configuration, env);
        services.AddDBContexts(configuration, env);
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddPresistance(configuration, env);
        return services;
    }
}
