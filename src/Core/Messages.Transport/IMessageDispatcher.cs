namespace CustomCode.Core.Messages.Transport
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for an <see cref="IMessage"/> dispatcher.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageDispatcher<T> : IDisposable
    {
        /// <summary>
        /// Connect a dispatchable instance (e.g. an <see cref="IMessageBus"/>) with this dispatcher.
        /// </summary>
        /// <param name="dispatchable"> The instance to be dispatched. </param>
        void Connect(T dispatchable);

        /// <summary>
        /// Asynchronously start the dispatcher.
        /// </summary>
        /// <returns> A task that can be awaited for the dispatcher to finish starting. </returns>
        Task StartAsync();

        /// <summary>
        /// Asynchronously stop the dispatcher.
        /// </summary>
        /// <returns> A task that can be awaited for the disptacher to finish stopping. </returns>
        Task StopAsync();
    }
}