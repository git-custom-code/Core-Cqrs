namespace CustomCode.Core.Messages.Transport
{
    using System;

    /// <summary>
    /// Interface for an observable stream of <see cref="IMessage"/>'s.
    /// </summary>
    public interface IMessageStream : IObservable<IMessage>
    { }
}