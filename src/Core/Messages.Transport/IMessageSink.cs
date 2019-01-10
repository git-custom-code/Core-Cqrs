namespace CustomCode.Core.Messages.Transport
{
    using System;

    /// <summary>
    /// Base interface for a component that can be used to publish an <see cref="IMessage"/> to all
    /// connected <see cref="IMessageStream{T}"/> instances using the configured transport protocol.
    /// </summary>
    /// <typeparam name="TMessageCategory">
    /// The category can be used to restrict the type of messages that can be published by this sink
    /// (e.g. only messages that implement a common base interface).
    /// </typeparam>
    public interface IMessageSink<TMessageCategory> : IDisposable where TMessageCategory : IMessage
    {
        /// <summary>
        /// Publish a new message of type <typeparamref name="T"/> to all connected
        /// <see cref="IMessageStream{T}"/> instances.
        /// </summary>
        /// <typeparam name="T"> The type of the message to be published. </typeparam>
        /// <param name="message"> The message to be published. </param>
        void Publish<T>(T message) where T : TMessageCategory;
    }
}