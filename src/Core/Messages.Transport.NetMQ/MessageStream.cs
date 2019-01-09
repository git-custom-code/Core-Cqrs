namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using Extensions;
    using global::NetMQ;
    using Serialization;
    using System;
    using System.Reactive;

    /// <summary>
    /// An observable stream of <see cref="IMessage"/>'s of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the stream's messages. </typeparam>
    public sealed class MessageStream<T> : ObservableBase<T>, IMessageStream<T> where T : IMessage
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="MessageStream{T}"/> type.
        /// </summary>
        /// <param name="deserializer">
        /// An <see cref="IMessageDeserializer"/> implementation that can be used to deserialize messages
        /// from the specified <paramref name="source"/>.
        /// </param>
        /// <param name="source">
        /// A reactive source that pushes a new <see cref="NetMQMessage"/> to the stream every time one is received
        /// at the connected <see cref="NetMQSocket"/>.
        /// </param>
        public MessageStream(IMessageDeserializer deserializer, IMessageSource source)
        {
            Deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            MessageType = typeof(T).Name;
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Gets an <see cref="IMessageDeserializer"/> implementation that can be used to deserialize messages
        /// from the specified <see cref="Source"/>.
        /// </summary>
        private IMessageDeserializer Deserializer { get; }

        /// <summary>
        /// Gets a reactive source that pushes a new <see cref="NetMQMessage"/> to the stream every time one is received
        /// at the connected <see cref="NetMQSocket"/>.
        /// </summary>
        private IMessageSource Source { get; }

        #endregion

        #region Data

        /// <summary>
        /// Gets the name of the stream's message type.
        /// </summary>
        private string MessageType { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        protected override IDisposable SubscribeCore(IObserver<T> observer)
        {
            return Source.Subscribe(
                multiFrameMessage =>
                    {
                        if (MessageType.Equals(multiFrameMessage.MessageType(), StringComparison.Ordinal))
                        {
                            var message = Deserializer.Deserialize<T>(multiFrameMessage.SerializedMessage());
                            observer.OnNext(message);
                        }
                    },
                observer.OnError,
                observer.OnCompleted);
        }

        #endregion
    }
}