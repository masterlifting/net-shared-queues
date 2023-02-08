using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Net.Shared.Queues.Abstractions.Connection;
using Net.Shared.Queues.Domain.RabbitMq.Connection;
using Net.Shared.Queues.Settings.RabbitMq;

namespace Net.Shared.Queues;

public static class Registrations
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSection>(_ => configuration.GetSection(RabbitMqSection.Name));
        services.AddSingleton<RabbitMqClient>();
        services.AddTransient<IMqConsumer, RabbitMqConsumer>();
        services.AddTransient<IMqProducer, RabbitMqProducer>();
    }
}
