namespace CustomCode.Core.Messages.Transport
{
    /// <summary>
    /// Implementation of an unfiltered <see cref="IEventStream"/>.
    /// </summary>
    public sealed class EventStream : MessageStream<IEvent>, IEventStream
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="EventStream"/> type.
        /// </summary>
        /// <param name="unfilteredMessageStream"> A stream of unfiltered <see cref="IMessage"/> instances. </param>
        public EventStream(IMessageStream unfilteredMessageStream)
            : base(unfilteredMessageStream)
        { }

        #endregion
    }
}