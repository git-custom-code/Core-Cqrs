namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a publisher to a <see cref="DualSocketBus"/> that can send <see cref="IMessage"/>
    /// instances to the subscribed <see cref="IMessageBus"/>.
    /// </summary>
    public interface IDualSocketBusPublisher : IObserver<NetMQMessage>, IDisposable
    {
        /// <summary>
        /// Asynchronously start the publisher (by connecting it to the underlying <see cref="IMessageDispatcher{T}"/>
        /// and ensuring that the dispatcher instance is up and running).
        /// </summary>
        /// <returns> A task that can be awaited for the publisher to finish starting. </returns>
        Task StartAsync();

        /// <summary>
        /// Asynchronously start the publisher (by connecting it to the underlying <see cref="IMessageDispatcher{T}"/>
        /// and ensuring that the dispatcher instance is up and running).
        /// </summary>
        /// <param name="cancellationToken"> A token that can be used to cancel the publisher's start routine. </param>
        /// <returns> A task that can be awaited for the publisher to finish starting. </returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously stop the publisher (by disconnecting it from the underlying <see cref="IMessageDispatcher{T}"/>
        /// and stopping the dispatcher instance if no other enpoints are connected to it).
        /// </summary>
        /// <returns> A task that can be awaited for the publisher to finish stopping. </returns>
        Task StopAsync();

        /// <summary>
        /// Asynchronously stop the publisher (by disconnecting it from the underlying <see cref="IMessageDispatcher{T}"/>
        /// and stopping the dispatcher instance if no other enpoints are connected to it).
        /// </summary>
        /// <param name="cancellationToken"> A token that can be used to cancel the publisher's stop routine. </param>
        /// <returns> A task that can be awaited for the publisher to finish stopping. </returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}