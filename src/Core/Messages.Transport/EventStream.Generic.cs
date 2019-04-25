namespace CustomCode.Core.Messages.Transport
{
    /// <summary>
    /// Implementation of an <see cref="IEventStream{T}"/> for filtered events of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the filtered events. </typeparam>
    public sealed class EventStream<T> : MessageStream<T>, IEventStream<T> where T : IEvent
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="EventStream{T}"/> type.
        /// </summary>
        /// <param name="unfilteredMessageStream"> A stream of unfiltered <see cref="IMessage"/> instances. </param>
        public EventStream(IMessageStream unfilteredMessageStream)
            : base(unfilteredMessageStream)
        { }

        #endregion
    }
}