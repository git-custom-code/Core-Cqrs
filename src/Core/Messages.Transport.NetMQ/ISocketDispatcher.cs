namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;

    /// <summary>
    /// An <see cref="IMessageDispatcher{T}"/> specialization for netMQ sockets.
    /// </summary>
    public interface ISocketDispatcher : IMessageDispatcher<ISocketPollable>
    { }
}