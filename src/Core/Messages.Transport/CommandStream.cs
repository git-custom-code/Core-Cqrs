namespace CustomCode.Core.Messages.Transport
{
    /// <summary>
    /// Implementation of an unfiltered <see cref="ICommandStream"/>.
    /// </summary>
    public sealed class CommandStream : MessageStream<ICommand>, ICommandStream
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="CommandStream"/> type.
        /// </summary>
        /// <param name="unfilteredMessageStream"> A stream of unfiltered <see cref="IMessage"/> instances. </param>
        public CommandStream(IMessageStream unfilteredMessageStream)
            : base(unfilteredMessageStream)
        { }

        #endregion
    }
}