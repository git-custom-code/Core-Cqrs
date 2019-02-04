namespace CustomCode.Core.Messages.Transport.NetMQ.Endpoints
{
    using global::NetMQ;

    /// <summary>
    /// An <see cref="IEndpoint"/> implementation that uses <see cref="NetMQSocket"/> to send and/or receive
    /// <see cref="IMessage"/> instances over tcp. 
    /// </summary>
    public struct TcpEndpoint : ISocketEndpoint
    {
        #region Dependencie

        /// <summary>
        /// Creates a new instance of the <see cref="TcpEndpoint"/> type.
        /// </summary>
        /// <param name="portNumber"> The underlying local socket's port number. </param>
        public TcpEndpoint(ushort portNumber)
            : this(IPAddress.LocalHost, new Port(portNumber))
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="TcpEndpoint"/> type.
        /// </summary>
        /// <param name="port"> The underlying local socket's port. </param>
        public TcpEndpoint(Port port)
            : this(IPAddress.LocalHost, port)
        { }

        /// <summary>
        /// Creates a new instnace of the <see cref="TcpEndpoint"/> type.
        /// </summary>
        /// <param name="ipAddress"> The endpoint's network ip address. </param>
        /// <param name="port"> The endpoint's network port. </param>
        public TcpEndpoint(IPAddress ipAddress, Port port)
        {
            IPAddress = ipAddress;
            Port = port;
            Protocol = Protocol.Tcp;
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets the endpoint's network ip address.
        /// </summary>
        public IPAddress IPAddress { get; }

        /// <summary>
        /// Gets the endpoint's network port.
        /// </summary>
        public Port Port { get; }

        /// <inheritdoc />
        public Protocol Protocol { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public string AsString()
        {
            return $"tcp://{IPAddress}:{Port}";
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return AsString();
        }

        #endregion
    }
}