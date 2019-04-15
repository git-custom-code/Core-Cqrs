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
    /// Implementation of a publisher for a <see cref="DualSocketBus"/>.
    /// </summary>
    public abstract class DualSocketBusPublisher : IDualSocketBusPublisher
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="DualSocketBusPublisher"/> type.
        /// </summary>
        /// <param name="messageBusEndpoint"> The subscribed message bus' endpoint. </param>
        /// <param name="dispatcher"> The dispatcher that is used for outgoing messages from the subscibed bus. </param>
        public DualSocketBusPublisher(
            IDualSocketEndpoint messageBusEndpoint,
            ISocketDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            MessageBusEndpoint = messageBusEndpoint;
            OutgoingMessages = new Lazy<PublisherSocket>(() =>
                {
                    var socket = new PublisherSocket();
                    socket.Connect(MessageBusEndpoint.Incoming);
                    return socket;
                }, true);
            OutgoingMessageQueue = new Lazy<NetMQQueue<NetMQMessage>>(() =>
                {
                    var queue = new NetMQQueue<NetMQMessage>();
                    queue.ReceiveReady += OnMessageQueued;
                    return queue;
                }, true);
        }

        /// <summary>
        /// Gets the dispatcher that is used for incoming messages from the subscribed bus.
        /// </summary>
        private ISocketDispatcher Dispatcher { get; }

        /// <summary>
        /// Gets the socket that will be used to publish outgoing messages.
        /// </summary>
        private Lazy<PublisherSocket> OutgoingMessages { get; }

        #endregion

        #region Data

        /// <summary>
        /// Gets the subscribed message bus' endpoint.
        /// </summary>
        private IDualSocketEndpoint MessageBusEndpoint { get; }

        /// <summary>
        /// Gets the queue that will enqueue outgoing messages in a thread-safe manner.
        /// </summary>
        private Lazy<NetMQQueue<NetMQMessage>> OutgoingMessageQueue { get; }

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
                if (OutgoingMessages.IsValueCreated)
                {
                    Dispatcher.Disconnect(OutgoingMessages.Value);
                    OutgoingMessages.Value.Dispose();
                }
                if (OutgoingMessageQueue.IsValueCreated)
                {
                    Dispatcher.Disconnect(OutgoingMessageQueue.Value);
                    OutgoingMessageQueue.Value.Dispose();
                }
            }
            finally
            {
                SyncLock.Release();
            }

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void OnCompleted()
        {
            StopAsync(); // Fire and forget here
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        {
            // do nothing here
        }

        private void OnMessageQueued(object sender, NetMQQueueEventArgs<NetMQMessage> e)
        {
            for (var i = 0u; i < 100u; ++i)
            {
                if (e.Queue.TryDequeue(out var message, TimeSpan.FromMilliseconds(10)))
                {
                    try
                    {
                        OutgoingMessages.Value.SendMultipartMessage(message);
                    }
                    catch (Exception exception)
                    {
                        OnError(exception);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <inheritdoc />
        public void OnNext(NetMQMessage value)
        {
            if (OutgoingMessageQueue.IsValueCreated == false || OutgoingMessages.IsValueCreated == false) // note that this is not thread-safe, but StartAsync is
            {
                StartAsync(); // we will fire and forget here
            }

            OutgoingMessageQueue.Value.Enqueue(value);
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
                Dispatcher.Connect(OutgoingMessages.Value);
                Dispatcher.Connect(OutgoingMessageQueue.Value);
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
                Dispatcher.Disconnect(OutgoingMessages.Value);
                Dispatcher.Disconnect(OutgoingMessageQueue.Value);
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

        #endregion
    }
}