using Apache.NMS;
using EasyAMQ.Abstraction;
using EasyAMQ.WebExample.Models;

namespace EasyAMQ.WebExample.Services;

public class ExampleProducer : IProducer<ExampleMessage>
{
    public IMessageProducer MessageProducer { get; set; } = default!;

    public async Task CreateMessage(ExampleMessage message)
    {
        var msg = await MessageProducer.CreateTextMessageAsync(message.Value);
        await MessageProducer.SendAsync(msg);
    }
}