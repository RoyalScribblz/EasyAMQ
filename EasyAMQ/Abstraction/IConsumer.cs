using Apache.NMS;

namespace EasyAMQ.Abstraction;

public interface IConsumer
{
    Task ProcessMessage(IMessage message);
}