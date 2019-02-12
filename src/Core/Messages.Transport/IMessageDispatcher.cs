namespace CustomCode.Core.Messages.Transport
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for an <see cref="IMessage"/> dispatcher.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageDispatcher<T> : IDisposable
    {
        /// <summary>
        /// Connect an endpoint (e.g. a network socket) with this dispatcher.
        /// </summary>
        /// <param name="endpoint"> The endpoint to be dispatched. </param>
        void Connect(T endpoint);

        /// <summary>
        /// Disconnect an endpoint (e.g. a network socket) from this dispatcher.
        /// </summary>
        /// <param name="endpoint"> The endpoint that should no longer be dispatched. </param>
        void Disconnect(T endpoint);

        /// <summary>
        /// Gets the number of connected endpoints that are currently dispatched.
        /// </summary>
        uint EndpointCount { get; }

        /// <summary>
        /// Gets the (unique) identity of this dispatcher instance.
        /// </summary>
        Identity<string> Id { get; }

        /// <summary>
        /// Gets a flag indicating whether or not the dispatcher has been started.
        /// </summary>
        bool IsDispatching { get; }

        /// <summary>
        /// Asynchronously start the dispatcher.
        /// </summary>
        /// <returns> A task that can be awaited for the dispatcher to finish starting. </returns>
        Task StartAsync();

        /// <summary>
        /// Asynchronously start the dispatcher.
        /// </summary>
        /// <param name="cancellationToken"> A token that can be used to cancel the dispatcher's start routine. </param>
        /// <returns> A task that can be awaited for the dispatcher to finish starting. </returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously stop the dispatcher.
        /// </summary>
        /// <returns> A task that can be awaited for the disptacher to finish stopping. </returns>
        Task StopAsync();

        /// <summary>
        /// Asynchronously stop the dispatcher.
        /// </summary>
        /// <param name="cancellationToken"> A token that can be used to cancel the dispatcher's stop routine. </param>
        /// <returns> A task that can be awaited for the disptacher to finish stopping. </returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}