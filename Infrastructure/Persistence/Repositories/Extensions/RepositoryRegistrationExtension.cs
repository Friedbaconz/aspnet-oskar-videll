using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.Users;
using Infrastructure.Persistence.Repositories.Memberships;
using Infrastructure.Persistence.Repositories.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Repositories.Extensions;

public static class RepositoryRegistrationExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddScoped<IMembershipRepository, MembershipRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
