namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;
    using System;

    public interface IMessageSource : IObservable<NetMQMessage>
    { }
}