namespace CustomCode.Core.Messages.Transport
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a message bus that can receive serialized <see cref="IMessage"/> instances from
    /// connected publishers and dispatch to connected subscribers.
    /// </summary>
    public interface IMessageBus : IDisposable
    {
        /// <summary>
        /// Asynchronously start the message bus (by connecting it to the underlying <see cref="IMessageDispatcher{T}"/>
        /// and ensuring that the dispatcher instance is up and running).
        /// </summary>
        /// <returns> A task that can be awaited for the bus to finish starting. </returns>
        Task StartAsync();

        /// <summary>
        /// Asynchronously start the message bus (by connecting it to the underlying <see cref="IMessageDispatcher{T}"/>
        /// and ensuring that the dispatcher instance is up and running).
        /// </summary>
        /// <param name="cancellationToken"> A token that can be used to cancel the bus' start routine. </param>
        /// <returns> A task that can be awaited for the bus to finish starting. </returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously stop the message bus (by disconnecting it from the underlying <see cref="IMessageDispatcher{T}"/>
        /// and stopping the dispatcher instance if no other enpoints are connected to it).
        /// </summary>
        /// <returns> A task that can be awaited for the bus to finish stopping. </returns>
        Task StopAsync();

        /// <summary>
        /// Asynchronously stop the message bus (by disconnecting it from the underlying <see cref="IMessageDispatcher{T}"/>
        /// and stopping the dispatcher instance if no other enpoints are connected to it).
        /// </summary>
        /// <param name="cancellationToken"> A token that can be used to cancel the bus' stop routine. </param>
        /// <returns> A task that can be awaited for the bus to finish stopping. </returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}