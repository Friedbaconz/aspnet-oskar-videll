using Infrastructure.Persistence.Contexts.Extensions;
using Infrastructure.Persistence.Repositories.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Extensions;

public static class PersistanceRegistrationExtension
{
    public static IServiceCollection AddPresistance(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddRepositories(configuration, env);
        services.AddDBContexts(configuration, env);
        return services;
    }
}
