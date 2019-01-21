namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;

    /// <summary>
    /// An <see cref="IMessageDispatcher{T}"/> specialization for netMQ based message dispatchers.
    /// </summary>
    public interface ISocketMessageDispatcher : IMessageDispatcher<ISocketPollable>
    { }
}