namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using Extensions;
    using global::NetMQ;
    using Serialization;
    using System;
    using System.Reactive;
    using System.Reactive.Linq;

    /// <summary>
    /// An observable stream of <see cref="IMessage"/>'s received from a .
    /// </summary>
    public sealed class MessageStream : ObservableBase<IMessage>, IMessageStream
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="MessageStream"/> type.
        /// </summary>
        /// <param name="serializedMessageStream">
        /// A reactive source that pushes a new <see cref="NetMQMessage"/> to the stream every time one is received
        /// at the connected <see cref="NetMQSocket"/>.
        /// </param>
        /// <param name="deserializer">
        /// An <see cref="IMessageDeserializer"/> implementation that can be used to deserialize messages
        /// from the specified <paramref name="serializedMessageStream"/>.
        /// </param>
        public MessageStream(IObservable<NetMQMessage> serializedMessageStream, IMessageDeserializer deserializer)
        {
            Deserializer = deserializer;
            DeserializedMessageStream = new Lazy<IObservable<IMessage>>(CreateDeserializedMessageStream, true);
            SerializedMessageStream = serializedMessageStream;
        }

        /// <summary>
        /// Gets an <see cref="IMessageDeserializer"/> implementation that can be used to deserialize messages
        /// from the specified <see cref="SerializedMessageStream"/>.
        /// </summary>
        private IMessageDeserializer Deserializer { get; }

        /// <summary>
        /// Gets a reactive source that pushes a new <see cref="NetMQMessage"/> to the stream every time one is received
        /// at the connected <see cref="NetMQSocket"/>.
        /// </summary>
        private IObservable<NetMQMessage> SerializedMessageStream { get; }

        #endregion

        #region Data

        /// <summary>
        /// Gets the internal observable deserialized <see cref="IMessage"/> stream.
        /// </summary>
        private Lazy<IObservable<IMessage>> DeserializedMessageStream { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        protected override IDisposable SubscribeCore(IObserver<IMessage> observer)
        {
            return DeserializedMessageStream.Value.Subscribe(observer);
        }

        /// <summary>
        /// Creates the deserialized <see cref="IMessage"/> stream and ensures that the <see cref="Deserializer"/>
        /// is invoked only once per reveived <see cref="NetMQMessage"/>.
        /// </summary>
        /// <returns> The observable deserialized <see cref="IMessage"/> stream. </returns>
        private IObservable<IMessage> CreateDeserializedMessageStream()
        {
            var observable = Observable.Create<IMessage>(observer =>
                {
                    return SerializedMessageStream.Subscribe(
                        multiFrameMessage =>
                        {
                            try
                            {
                                var message = Deserializer.Deserialize<IMessage>(multiFrameMessage.SerializedMessage());
                                observer.OnNext(message);
                            }
                            catch (Exception e)
                            {
                                observer.OnError(e);
                            }
                        },
                        observer.OnError,
                        observer.OnCompleted);
                });

            return observable.Publish().RefCount();
        }

        #endregion
    }
}