using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Application;
using WebApi.Infrastructure.RabbitMQ;
using WebApi.Infrastructure.Repository;

namespace WebApi.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        // services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}