namespace CustomCode.Core.Messages.Transport.NetMQ.Endpoints
{
    using global::NetMQ;

    /// <summary>
    /// An <see cref="IEndpoint"/> implementation that uses <see cref="NetMQSocket"/> to send and/or receive
    /// <see cref="IMessage"/> instances using in-process communication protocol. 
    /// </summary>
    public struct InProcessEndpoint : ISocketEndpoint
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="InProcessEndpoint"/> type.
        /// </summary>
        /// <param name="identity"> The endpoint's unique identity. </param>
        public InProcessEndpoint(string identity)
            : this(new Identity<string>(identity))
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="InProcessEndpoint"/> type.
        /// </summary>
        /// <param name="identity"> The endpoint's unique identity. </param>
        public InProcessEndpoint(Identity<string> identity)
        {
            Identity = identity;
            Protocol = Protocol.Tcp;
        }

        #endregion

        #region Data

        /// <inheritdoc />
        public Protocol Protocol { get; }

        /// <summary>
        /// Gets the endpoint's identity.
        /// </summary>
        public Identity<string> Identity { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public string AsString()
        {
            return $"inproc://{Identity}";
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return AsString();
        }

        #endregion
    }
}