namespace CustomCode.Core.Messages.Transport
{
    using System;
    using System.Reactive.Linq;

    /// <summary>
    /// Implementation of a <see cref="IMessageStream{T}"/> for filtered messages of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the filtered messages. </typeparam>
    public class MessageStream<T> : IMessageStream<T> where T : IMessage
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="MessageStream{T}"/> type.
        /// </summary>
        /// <param name="unfilteredMessageStream"> A stream of unfiltered <see cref="IMessage"/> instances. </param>
        public MessageStream(IMessageStream unfilteredMessageStream)
        {
            UnfilteredMessageStream = unfilteredMessageStream;
        }

        /// <summary>
        /// Gets a stream of unfiltered <see cref="IMessage"/> instances.
        /// </summary>
        private IMessageStream UnfilteredMessageStream { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return UnfilteredMessageStream.Subscribe(
                message =>
                {
                    try
                    {
                        if (message is T filteredMessage)
                        {
                            observer.OnNext(filteredMessage);
                        }
                    }
                    catch(Exception e)
                    {
                        observer.OnError(e);
                    }
                },
                observer.OnError,
                observer.OnCompleted);
        }

        #endregion
    }
}