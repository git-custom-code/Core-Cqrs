namespace CustomCode.Core.Messages.Transport.NetMQ.Endpoints
{
    using global::NetMQ;
    using System;

    /// <summary>
    /// An <see cref="IEndpoint"/> implementation that uses dual <see cref="NetMQSocket"/>s to send and receive
    /// <see cref="IMessage"/> instances using in-process communication protocol.
    /// </summary>
    public struct DualInProcessEndpoint : IDualSocketEndpoint
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="DualInProcessEndpoint"/> type.
        /// </summary>
        /// <param name="identity"> The endpoint's unique identity. </param>
        public DualInProcessEndpoint(string identity)
            : this(new Identity<string>($"{identity}-in"), new Identity<string>($"{identity}-out"))
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="DualInProcessEndpoint"/> type.
        /// </summary>
        /// <param name="identity"> The endpoint's unique identity. </param>
        public DualInProcessEndpoint(Identity<string> identity)
            : this(new Identity<string>($"{identity.Value}-in"), new Identity<string>($"{identity.Value}-out"))
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="DualInProcessEndpoint"/> type.
        /// </summary>
        /// <param name="incoming"> The endpoint's unique identity for incoming messages. </param>
        /// <param name="outgoing"> The endpoint's unique identity for outgoing messages. </param>
        public DualInProcessEndpoint(Identity<string> incoming, Identity<string> outgoing)
        {
            if (incoming == outgoing)
            {
                throw new ArgumentException();
            }
            
            Incoming = new InProcessEndpoint(incoming);
            Outgoing = new InProcessEndpoint(outgoing);
            Protocol = Protocol.InProc;
        }

        /// <inheritdoc />
        public ISocketEndpoint Incoming { get; }

        /// <inheritdoc />
        public ISocketEndpoint Outgoing { get; }

        #endregion

        #region Data
        
        /// <inheritdoc />
        public Protocol Protocol { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public override string ToString()
        {
            return $"inproc:// | In:{((InProcessEndpoint)Incoming).Identity} | Out:{((InProcessEndpoint)Outgoing).Identity}";
        }

        #endregion
    }
}