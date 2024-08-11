using Apache.NMS;
using EasyAMQ.Abstraction;

namespace EasyAMQ.WebExample.Services;

public class ExampleConsumer : IConsumer
{
    public Task ProcessMessage(IMessage message)
    {
        if (!message.IsBodyAssignableTo(typeof(string)))
        {
            return Task.CompletedTask;
        }

        var exampleMessage = message.Body<string>();
        
        Console.WriteLine(exampleMessage);
        
        message.Acknowledge();
        
        return Task.CompletedTask;
    }
}