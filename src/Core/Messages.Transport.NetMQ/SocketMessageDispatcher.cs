namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementation of the <see cref="IMessageDispatcher{T}"/> interface for netMQ based message dispatchers.
    /// </summary>
    public abstract class SocketMessageDispatcher : ISocketMessageDispatcher
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="SocketMessageDispatcher"/> type.
        /// </summary>
        /// <param name="id"></param>
        protected SocketMessageDispatcher(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Id = $"{nameof(SocketMessageDispatcher)}-{Guid.NewGuid()}";
            }
            else
            {
                Id = id;
            }
        }

        /// <summary>
        /// Gets the internal <see cref="NetMQPoller"/> that will run on its own background thread and dispatch
        /// messages from the connected sockets.
        /// </summary>
        private Lazy<NetMQPoller> PollingThread { get; set; } = new Lazy<NetMQPoller>(() => new NetMQPoller(), true);

        /// <summary>
        /// Gets an async/await compatible synchronization primitive. 
        /// </summary>
        private SemaphoreSlim SyncLock { get; } = new SemaphoreSlim(1, 1);

        #endregion

        #region Data

        /// <summary>
        /// Gets a thread-safe collection of connected <see cref="ISocketPollable"/> that are dispatched by this instance. 
        /// </summary>
        private ConcurrentBag<ISocketPollable> DispatchedSockets { get; } = new ConcurrentBag<ISocketPollable>();

        /// <summary>
        /// Gets the (unique) identiy of this dispatcher instance.
        /// </summary>
        public string Id { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public void Connect(ISocketPollable dispatchable)
        {
            DispatchedSockets.Add(dispatchable);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            SyncLock.Wait();
            try
            {
                if (PollingThread?.IsValueCreated == true)
                {
                    foreach (var socket in DispatchedSockets)
                    {
                        PollingThread.Value.Remove(socket);
                    }
                    PollingThread.Value.Dispose();
                    PollingThread = null;
                }
            }
            finally
            {
                SyncLock.Release();
            }

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public async Task StartAsync()
        {
            if (PollingThread?.IsValueCreated == true && PollingThread?.Value.IsRunning == true)
            {
                return;
            }

            await SyncLock.WaitAsync();
            try
            {
                if (PollingThread == null)
                {
                    throw new ObjectDisposedException(nameof(SocketMessageDispatcher));
                }

                if (PollingThread.IsValueCreated == false)
                {
                    await Task.Run(() =>
                        {
                            var poller = PollingThread.Value;
                            foreach (var socket in DispatchedSockets)
                            {
                                poller.Add(socket);
                            }
                            poller.RunAsync();
                        });
                    return;
                }
            }
            finally
            {
                SyncLock.Release();
            }
        }

        /// <inheritdoc />
        public async Task StopAsync()
        {
            if (PollingThread == null || PollingThread.IsValueCreated == false)
            {
                return;
            }

            await SyncLock.WaitAsync();
            try
            {
                if (PollingThread?.IsValueCreated == true && PollingThread?.Value.IsRunning == true)
                {
                    await Task.Run(() => PollingThread.Value.Stop());
                    PollingThread = new Lazy<NetMQPoller>(() => new NetMQPoller(), true);
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