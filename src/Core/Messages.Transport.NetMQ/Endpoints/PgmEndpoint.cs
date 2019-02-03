namespace CustomCode.Core.Messages.Transport.NetMQ.Endpoints
{
    using global::NetMQ;

    /// <summary>
    /// An <see cref="IEndpoint"/> implementation that uses <see cref="NetMQSocket"/> to send and/or receive
    /// <see cref="IMessage"/> instances over pgm. 
    /// </summary>
    public sealed class PgmEndpoint : ISocketEndpoint
    {
        #region Dependencie

        /// <summary>
        /// Creates a new instance of the <see cref="PgmEndpoint"/> type.
        /// </summary>
        /// <param name="portNumber"> The underlying local socket's port number. </param>
        public PgmEndpoint(ushort portNumber)
            : this(new IPAddress(127, 0, 0, 1), new Port(portNumber))
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="PgmEndpoint"/> type.
        /// </summary>
        /// <param name="port"> The underlying local socket's port. </param>
        public PgmEndpoint(Port port)
            : this (new IPAddress(127, 0, 0, 1), port)
        { }

        /// <summary>
        /// Creates a new instnace of the <see cref="PgmEndpoint"/> type.
        /// </summary>
        /// <param name="ipAddress"> The endpoint's network ip address. </param>
        /// <param name="port"> The endpoint's network port. </param>
        public PgmEndpoint(IPAddress ipAddress, Port port)
        {
            IPAddress = ipAddress;
            Port = port;
        }

        #endregion

        #region Data

        /// <inheritdoc />
        public Protocol Protocol { get; } = Protocol.Pgm;

        /// <summary>
        /// Gets the endpoint's network ip address.
        /// </summary>
        public IPAddress IPAddress { get; }

        /// <summary>
        /// Gets the endpoint's network port.
        /// </summary>
        public Port Port { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public string AsString()
        {
            return $"pgm://{IPAddress}:{Port}";
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return AsString();
        }

        #endregion
    }
}