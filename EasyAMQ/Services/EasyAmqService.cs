using Amqp;
using Apache.NMS;
using Apache.NMS.Util;
using EasyAMQ.Abstraction;
using EasyAMQ.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using IConnection = Apache.NMS.IConnection;
using IConnectionFactory = Apache.NMS.IConnectionFactory;
using ISession = Apache.NMS.ISession;

namespace EasyAMQ.Services;

public class EasyAmqService : IHostedService, IDisposable
{
    private readonly IServiceProvider _provider;
    private readonly IConnection _connection;
    private readonly ISession _session;

    // ReSharper disable once CollectionNeverQueried.Local
    private readonly Dictionary<IConsumer, IMessageConsumer> _consumers = new();
    // ReSharper disable once CollectionNeverQueried.Local
    private readonly Dictionary<IProducer, IMessageProducer> _producers = new();

    public EasyAmqService(IServiceProvider provider)
    {
        _provider = provider;
        
        var factory = provider.GetRequiredService<IConnectionFactory>();
        var options = provider.GetRequiredService<IOptions<EasyAmqOptions>>().Value;
        
        _connection = factory.CreateConnection(options.Username, options.Password);
        _session = _connection.CreateSession();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _connection.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _connection.StopAsync();
    }

    public void Dispose()
    {
        _session.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }

    public EasyAmqService AddConsumer<TConsumer, TMessage>() where TConsumer : IConsumer, new()
    {
        var consumer = new TConsumer();
        
        var destination = SessionUtil.GetDestination(_session, $"queue://{typeof(TMessage).Name}");
            
        var messageConsumer = _session.CreateConsumer(destination);

        messageConsumer.Listener += message => consumer.ProcessMessage(message);

        _consumers[consumer] = messageConsumer;
        
        return this;
    }

    public EasyAmqService AddProducer<TProducer, TMessage>() where TProducer : class, IProducer<TMessage>, new()
    {
        var producer = _provider.GetRequiredService<TProducer>();
        
        var destination = SessionUtil.GetDestination(_session, $"queue://{typeof(TMessage).Name}");
        
        var messageProducer = _session.CreateProducer(destination);

        producer.MessageProducer = messageProducer;
        
        _producers[producer] = messageProducer;

        return this;
    }
}