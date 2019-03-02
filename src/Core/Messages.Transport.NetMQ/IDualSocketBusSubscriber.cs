namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a subscriber to a <see cref="DualSocketBus"/> that can receive serialized <see cref="IMessage"/>
    /// instances from the subscribed <see cref="IMessageBus"/>.
    /// </summary>
    public interface IDualSocketBusSubscriber : IObservable<NetMQMessage>, IDisposable
    {
        /// <summary>
        /// Asynchronously start the subscriber (by connecting it to the underlying <see cref="IMessageDispatcher{T}"/>
        /// and ensuring that the dispatcher instance is up and running).
        /// </summary>
        /// <returns> A task that can be awaited for the subcriber to finish starting. </returns>
        Task StartAsync();

        /// <summary>
        /// Asynchronously start the subscriber (by connecting it to the underlying <see cref="IMessageDispatcher{T}"/>
        /// and ensuring that the dispatcher instance is up and running).
        /// </summary>
        /// <param name="cancellationToken"> A token that can be used to cancel the subscriber's start routine. </param>
        /// <returns> A task that can be awaited for the subcriber to finish starting. </returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously stop the subscriber (by disconnecting it from the underlying <see cref="IMessageDispatcher{T}"/>
        /// and stopping the dispatcher instance if no other enpoints are connected to it).
        /// </summary>
        /// <returns> A task that can be awaited for the subscriber to finish stopping. </returns>
        Task StopAsync();

        /// <summary>
        /// Asynchronously stop the subscriber (by disconnecting it from the underlying <see cref="IMessageDispatcher{T}"/>
        /// and stopping the dispatcher instance if no other enpoints are connected to it).
        /// </summary>
        /// <param name="cancellationToken"> A token that can be used to cancel the subscriber's stop routine. </param>
        /// <returns> A task that can be awaited for the subscriber to finish stopping. </returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}