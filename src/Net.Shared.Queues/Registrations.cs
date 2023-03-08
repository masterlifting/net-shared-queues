using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Net.Shared.Queues.Abstractions.Core.MessageQueue;
using Net.Shared.Queues.Models.Settings.MessageQueue.RabbitMq;
using Net.Shared.Queues.RabbitMq;

namespace Net.Shared.Queues;

public static class Registrations
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSection>(_ => configuration.GetSection(RabbitMqSection.Name));
        services.AddSingleton<RabbitMqClient>();
        services.AddTransient<IMqConsumer, RabbitMqConsumer_Old>();
        services.AddTransient<IMqProducer, RabbitMqProducer>();
    }
}
