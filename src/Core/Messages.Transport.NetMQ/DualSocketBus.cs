namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using Endpoints;
    using Extensions;
    using global::NetMQ;
    using global::NetMQ.Sockets;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// An <see cref="IMessageBus"/> implementation that uses two separate <see cref="NetMQSocket"/>s:
    /// A <see cref="SubscriberSocket"/> for receiving incoming serialized messages and a <see cref="PublisherSocket"/>
    /// for sending outgoing serialized messages.
    /// </summary>
    public abstract class DualSocketBus : IMessageBus
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="DualSocketBus"/> type.
        /// </summary>
        /// <param name="endpoint"> The message bus' endpoint. </param>
        /// <param name="dispatcher"> The dispatcher that is used for incoming and outgoing messages. </param>
        protected DualSocketBus(IDualSocketEndpoint endpoint, ISocketDispatcher dispatcher)
        {
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            IncomingMessages = new Lazy<SubscriberSocket>(() =>
                {
                    var incomingMessages = new SubscriberSocket();
                    incomingMessages.SubscribeToAnyTopic();
                    incomingMessages.Bind(Endpoint.Incoming);
                    incomingMessages.ReceiveReady += OnIncomingMessagesReceived;
                    return incomingMessages;
                }, true);
            OutgoingMessages = new Lazy<PublisherSocket>(() =>
                {
                    var outgoingMessages = new PublisherSocket();
                    outgoingMessages.Bind(Endpoint.Outgoing);
                    return outgoingMessages;
                }, true);
        }

        /// <summary>
        /// Gets the dispatcher that is used for incoming and outgoing messages.
        /// </summary>
        private ISocketDispatcher Dispatcher { get; }

        /// <summary>
        /// Gets the socket that will receive incoming messages.
        /// </summary>
        private Lazy<SubscriberSocket> IncomingMessages { get; }

        /// <summary>
        /// Gets the socket that will send outgoing messages.
        /// </summary>
        private Lazy<PublisherSocket> OutgoingMessages { get; }

        #endregion

        #region Data

        /// <summary>
        /// Gets the bus' endpoint.
        /// </summary>
        private IDualSocketEndpoint Endpoint { get; }

        /// <summary>
        /// Gets an async/await compatible synchronization primitive. 
        /// </summary>
        private SemaphoreSlim SyncLock { get; } = new SemaphoreSlim(1, 1);

        #endregion

        #region Logic

        /// <inheritdoc />
        public void Dispose()
        {
            SyncLock.Wait();
            try
            {
                if (IncomingMessages.IsValueCreated)
                {
                    Dispatcher.Disconnect(IncomingMessages.Value);
                    IncomingMessages.Value.Dispose();
                }
                if (OutgoingMessages.IsValueCreated)
                {
                    Dispatcher.Disconnect(OutgoingMessages.Value);
                    OutgoingMessages.Value.Dispose();
                }
            }
            finally
            {
                SyncLock.Release();
            }

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public Task StartAsync()
        {
            return StartAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SyncLock.WaitAsync(cancellationToken);
            try
            {
                Dispatcher.Connect(IncomingMessages.Value);
                Dispatcher.Connect(OutgoingMessages.Value);
                if (Dispatcher.IsDispatching)
                {
                    return;
                }
                await Dispatcher.StartAsync(cancellationToken);
            }
            finally
            {
                SyncLock.Release();
            }
        }

        /// <inheritdoc />
        public Task StopAsync()
        {
            return StopAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await SyncLock.WaitAsync(cancellationToken);
            try
            {
                Dispatcher.Disconnect(IncomingMessages.Value);
                Dispatcher.Disconnect(OutgoingMessages.Value);
                if (Dispatcher.EndpointCount == 0)
                {
                    await Dispatcher.StopAsync(cancellationToken);
                }
            }
            finally
            {
                SyncLock.Release();
            }
        }

        /// <summary>
        /// Event that is raised when one or more serialized <see cref="IMessage"/>s were received
        /// on the <see cref="IncomingMessages"/> socket.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The arguments of the event. </param>
        private void OnIncomingMessagesReceived(object sender, NetMQSocketEventArgs e)
        {
            var message = new NetMQMessage();
            for (var i = 0u; i < 100u; ++i)
            {
                if (IncomingMessages.Value.TryReceiveMultipartMessage(ref message))
                {
                    OutgoingMessages.Value.SendMultipartMessage(message);
                }
                else
                {
                    break;
                }
            }
        }

        #endregion
    }
}