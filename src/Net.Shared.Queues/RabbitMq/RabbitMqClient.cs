using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Shared.Queues.Models.RabbitMq.Domain;
using Net.Shared.Queues.Models.RabbitMq.Settings;
using RabbitMQ.Client;

namespace Net.Shared.Queues.RabbitMq;

public sealed class RabbitMqClient
{
    private bool _isRegister;
    private readonly string _info;

    private readonly ILogger<RabbitMqClient> _logger;
    private readonly RabbitMqClientSettings _settings;
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMqClient(ILogger<RabbitMqClient> logger, IOptions<RabbitMqClientSettings> options)
    {
        _logger = logger;

        _settings = options.Value;

        _connectionFactory = new ConnectionFactory
        {
            HostName = _settings.Connection.Host,
            UserName = _settings.Connection.User,
            Password = _settings.Connection.Password
        };

        var objectId = GetHashCode();
        _info = $"RabbitMq client {objectId}";
    }

    public IModel CreateModel()
    {
        if (!_isRegister)
            Register();

        _logger.LogTrace($"{_info} connecting...");

        using var connection = _connectionFactory.CreateConnection();
        var model = connection.CreateModel();

        _logger.LogTrace($"{_info} connected.");

        return model;
    }

    private void Register()
    {
        using var model = CreateModel();

        _logger.LogTrace($"{_info} registering models...");

        foreach (var item in _settings.ModelBuilders)
            Register(model, item.Exchange, item.Queue);

        _isRegister = true;

        _logger.LogTrace($"{_info} has registered models.");
    }
    private static void Register(IModel model, RabbitMqExchange exchange, RabbitMqQueue queue)
    {
        var exchangeType = string.Intern(exchange.Type.ToString());
        var exchangeName = string.Intern(exchange.Name.ToString());
        var routingKey = string.Intern($"{exchangeName}.{queue.Name}.*");

        model.ExchangeDeclare(exchangeName, exchangeType, exchange.IsDurable, exchange.IsAutoDelete, exchange.Arguments);
        model.QueueDeclare(queue.Name, queue.IsDurable, queue.IsExclusive, queue.IsAutoDelete, queue.Arguments);

        model.QueueBind(queue.Name, exchangeName, routingKey);
    }
}