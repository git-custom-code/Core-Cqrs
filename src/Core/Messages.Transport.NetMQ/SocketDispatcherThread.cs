namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementation of the <see cref="IMessageDispatcher{T}"/> interface for netMQ sockets.
    /// </summary>
    public abstract class SocketDispatcherThread : ISocketDispatcherThread
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="SocketDispatcherThread"/> type.
        /// </summary>
        /// <param name="id"></param>
        protected SocketDispatcherThread(Identity<string>? id = null)
        {
            if (id.HasValue)
            {
                Id = id.Value;
            }
            else
            {
                Id = new Identity<string>($"{nameof(SocketDispatcherThread)}-{Guid.NewGuid()}");
            }
        }

        /// <summary>
        /// Gets the internal <see cref="NetMQPoller"/> that will run on its own background thread and dispatch
        /// messages from the connected sockets.
        /// </summary>
        private Lazy<NetMQPoller> PollingThread { get; set; } = new Lazy<NetMQPoller>(() => new NetMQPoller(), true);

        #endregion

        #region Data

        /// <inheritdoc />
        public Identity<string> Id { get; }

        /// <inheritdoc />
        public uint EndpointCount
        {
            get { return (uint)DispatchedSockets.Count; }
        }

        /// <inheritdoc />
        public bool IsDispatching
        {
            get { return PollingThread?.IsValueCreated == true && PollingThread?.Value.IsRunning == true; }
        }
        
        /// <summary>
        /// Gets a collection of connected <see cref="ISocketPollable"/> instances that are dispatched. 
        /// </summary>
        private HashSet<ISocketPollable> DispatchedSockets { get; } = new HashSet<ISocketPollable>();
        
        /// <summary>
        /// Gets an async/await compatible synchronization primitive. 
        /// </summary>
        private SemaphoreSlim SyncLock { get; } = new SemaphoreSlim(1, 1);

        #endregion

        #region Logic

        /// <inheritdoc />
        public void Connect(ISocketPollable endpoint)
        {
            if (DispatchedSockets.Contains(endpoint) == false)
            {
                SyncLock.Wait();
                try
                {
                    if (DispatchedSockets.Contains(endpoint) == false)
                    {
                        DispatchedSockets.Add(endpoint);
                        if (PollingThread?.IsValueCreated == true)
                        {
                            PollingThread.Value.Add(endpoint);
                        }
                    }
                }
                finally
                {
                    SyncLock.Release();
                }
            }
        }

        /// <inheritdoc />
        public void Disconnect(ISocketPollable endpoint)
        {
            if (DispatchedSockets.Contains(endpoint))
            {
                SyncLock.Wait();
                try
                {
                    if (DispatchedSockets.Contains(endpoint))
                    {
                        DispatchedSockets.Remove(endpoint);
                        if (PollingThread?.IsValueCreated == true)
                        {
                            PollingThread.Value.Remove(endpoint);
                        }
                    }
                }
                finally
                {
                    SyncLock.Release();
                }
            }
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
                    DispatchedSockets.Clear();
                    PollingThread.Value.Dispose();
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
            if (IsDispatching)
            {
                return;
            }

            await SyncLock.WaitAsync(cancellationToken);
            try
            {
                if (PollingThread == null)
                {
                    throw new ObjectDisposedException(
                        nameof(ISocketDispatcherThread), $"The dispatcher with the id {Id} was already disposed");
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
                        }, cancellationToken);
                    return;
                }
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
            if (PollingThread == null || PollingThread.IsValueCreated == false)
            {
                return;
            }

            await SyncLock.WaitAsync(cancellationToken);
            try
            {
                if (IsDispatching)
                {
                    await Task.Run(() => PollingThread.Value.Stop(), cancellationToken);
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