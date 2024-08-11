using Apache.NMS;

namespace EasyAMQ.Abstraction;

public interface IProducer<in TMessage> : IProducer
{
    public IMessageProducer MessageProducer { get; set; }
    Task CreateMessage(TMessage message);
}

public interface IProducer
{
}