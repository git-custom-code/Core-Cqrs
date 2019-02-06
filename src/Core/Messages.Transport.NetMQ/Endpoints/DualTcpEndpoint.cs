namespace CustomCode.Core.Messages.Transport.NetMQ.Endpoints
{
    using global::NetMQ;
    using System;

    /// <summary>
    /// An <see cref="IEndpoint"/> implementation that uses dual <see cref="NetMQSocket"/>s to send and receive
    /// <see cref="IMessage"/> instances over tcp. 
    /// </summary>
    public struct DualTcpEndpoint : IDualSocketEndpoint
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="DualTcpEndpoint"/> type.
        /// </summary>
        /// <param name="incomingPortNumber"> The endpoint's port number for incoming <see cref="IMessage"/>s. </param>
        /// <param name="outgoingPortNumber"> The endpoint's port number for outgoing <see cref="IMessage"/>s. </param>
        public DualTcpEndpoint(ushort incomingPortNumber, ushort outgoingPortNumber)
            : this (IPAddress.LocalHost, new Port(incomingPortNumber), new Port(outgoingPortNumber))
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="DualTcpEndpoint"/> type.
        /// </summary>
        /// <param name="incomingPort"> The endpoint's <see cref="Port"/> for incoming <see cref="IMessage"/>s. </param>
        /// <param name="outgoingPort"> The endpoint's <see cref="Port"/> for outgoing <see cref="IMessage"/>s. </param>
        public DualTcpEndpoint(Port incomingPort, Port outgoingPort)
            : this(IPAddress.LocalHost, incomingPort, outgoingPort)
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="DualTcpEndpoint"/> type.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="incomingPort"> The endpoint's <see cref="Port"/> for incoming <see cref="IMessage"/>s. </param>
        /// <param name="outgoingPort"> The endpoint's <see cref="Port"/> for outgoing <see cref="IMessage"/>s. </param>
        public DualTcpEndpoint(IPAddress address, Port incomingPort, Port outgoingPort)
        {
            if (incomingPort == outgoingPort)
            {
                throw new ArgumentException();
            }

            Incoming = new TcpEndpoint(address, incomingPort);
            IPAddress = address;
            Outgoing = new TcpEndpoint(address, outgoingPort);
            Protocol = Protocol.Tcp;
        }

        /// <inheritdoc />
        public ISocketEndpoint Incoming { get; }

        /// <inheritdoc />
        public ISocketEndpoint Outgoing { get; }

        #endregion

        #region Data

        /// <summary>
        /// Gets the endpoint's network ip address.
        /// </summary>
        public IPAddress IPAddress { get; }

        /// <inheritdoc />
        public Protocol Protocol { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public override string ToString()
        {
            return $"tcp://{IPAddress} | In:{((TcpEndpoint)Incoming).Port} | Out:{((TcpEndpoint)Outgoing).Port}";
        }

        #endregion
    }
}