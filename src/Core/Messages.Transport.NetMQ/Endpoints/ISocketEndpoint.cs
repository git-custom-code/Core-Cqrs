namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;

    /// <summary>
    /// Interface for a <see cref="NetMQSocket"/> endpoint.
    /// </summary>
    public interface ISocketEndpoint : IEndpoint
    {
        /// <summary>
        /// Gets the <see cref="Protocol"/> that is used by this endpoint.
        /// </summary>
        Protocol Protocol { get; }

        /// <summary>
        /// Converts the endpoint to NetMQ's string address format.
        /// </summary>
        /// <returns> The endpoint as NetMQ's string address format. </returns>
        string AsString();
    }
}