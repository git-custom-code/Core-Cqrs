namespace CustomCode.Core.Messages.Transport
{
    using System;
    using System.Reactive.Linq;

    /// <summary>
    /// Implementation of an <see cref="ICommandStream{T}"/> for filtered commands of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the filtered commands. </typeparam>
    public sealed class CommandStream<T> : MessageStream<T>, ICommandStream<T> where T : ICommand
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="CommandStream{T}"/> type.
        /// </summary>
        /// <param name="unfilteredMessageStream"> A stream of unfiltered <see cref="IMessage"/> instances. </param>
        public CommandStream(IMessageStream unfilteredMessageStream)
            : base(unfilteredMessageStream)
        { }

        #endregion
    }
}