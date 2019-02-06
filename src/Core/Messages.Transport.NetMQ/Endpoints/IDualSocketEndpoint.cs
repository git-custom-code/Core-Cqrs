namespace CustomCode.Core.Messages.Transport.NetMQ.Endpoints
{
    using global::NetMQ;

    /// <summary>
    /// Interface for a dual socket endpoint (e.g. an <see cref="IMessageBus"/>) using an incoming
    /// as well as an outgoing <see cref="NetMQSocket"/> socket.
    /// </summary>
    public interface IDualSocketEndpoint : IEndpoint
    {
        /// <summary>
        /// Gets the <see cref="Protocol"/> that is used by this endpoint.
        /// </summary>
        Protocol Protocol { get; }

        /// <summary>
        /// Gets the endpoint for incoming messages.
        /// </summary>
        ISocketEndpoint Incoming { get; }

        /// <summary>
        /// Gets the endpoint for outgoing messages.
        /// </summary>
        ISocketEndpoint Outgoing { get; }
    }
}