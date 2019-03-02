namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using Endpoints;
    using Extensions;
    using global::NetMQ;
    using global::NetMQ.Sockets;
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementation of a subcriber for a <see cref="DualSocketBus"/>.
    /// </summary>
    public abstract class DualSocketBusSubscriber : IDualSocketBusSubscriber
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="DualSocketBusSubscriber"/> type.
        /// </summary>
        /// <param name="messageBusEndpoint"> The subscribed message bus' endpoint. </param>
        /// <param name="dispatcher"> The dispatcher that is used for incoming messages from the subscibed bus. </param>
        /// <param name="scheduler">
        /// An optional scheduler that can be used to deserialize incoming <see cref="NetMQMessage"/> instances on a separate thread.
        /// </param>
        protected DualSocketBusSubscriber(
            IDualSocketEndpoint messageBusEndpoint,
            ISocketDispatcher dispatcher,
            IScheduler? scheduler = null)
        {
            Dispatcher = dispatcher;
            MessageBusEndpoint = messageBusEndpoint;
            IncomingMessages = new Lazy<SubscriberSocket>(() =>
                {
                    var incomingMessages = new SubscriberSocket();
                    incomingMessages.SubscribeToAnyTopic();
                    incomingMessages.Connect(MessageBusEndpoint.Outgoing);
                    incomingMessages.ReceiveReady += OnIncomingMessageReceived;
                    return incomingMessages;
                }, true);
            Scheduler = scheduler;
        }

        /// <summary>
        /// Gets the dispatcher that is used for incoming messages from the subscribed bus.
        /// </summary>
        private ISocketDispatcher Dispatcher { get; }

        /// <summary>
        /// Gets the socket that will receive incoming messages from the subscribed bus.
        /// </summary>
        private Lazy<SubscriberSocket> IncomingMessages { get; }

        /// <summary>
        /// Gets optional scheduler that can be used to deserialize incoming <see cref="NetMQMessage"/> instances on a separate thread.
        /// </summary>
        private IScheduler? Scheduler { get; }

        #endregion

        #region Data

        /// <summary>
        /// Gets the subscribed message bus' endpoint.
        /// </summary>
        private IDualSocketEndpoint MessageBusEndpoint { get; }

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

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<NetMQMessage> observer)
        {
            var observable = Observable
                .FromEvent<Action<NetMQMessage>, NetMQMessage>(
                    action =>
                        {
                            if (MessageReceived == null) // note that this is not thread-safe, but StartAsync is
                            {
                                StartAsync(); // we will fire and forget here
                            }
                            MessageReceived += action;
                        },
                    action =>
                        {
                            MessageReceived -= action;
                            if (MessageReceived == null) // note that this is not thread-safe, but StopAsync is
                            {
                                StopAsync();  // we will fire and forget here
                            }
                        });

            if (Scheduler != null)
            {
                observable = observable.ObserveOn(Scheduler);
            }

            return observable.Subscribe(observer);
        }

        /// <summary>
        /// Internal event that is raised whenever a new <see cref="NetMQMessage"/> was received from the
        /// subscribed <see cref="DualSocketBus"/>.
        /// </summary>
        private event Action<NetMQMessage> MessageReceived;

        /// <summary>
        /// Event that is raised when a new message from the <see cref="IncomingMessages"/> has been dispatched
        /// by the associated <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The arguments of the event. </param>
        private void OnIncomingMessageReceived(object sender, NetMQSocketEventArgs e)
        {
            var message = new NetMQMessage();
            for (var i = 0u; i < 100u; ++i)
            {
                if (IncomingMessages.Value.TryReceiveMultipartMessage(ref message, 2))
                {
                    MessageReceived?.Invoke(message);
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