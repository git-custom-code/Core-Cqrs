namespace CustomCode.Core.Messages.Transport
{
    using System;

    /// <summary>
    /// Interface for an observable stream of <see cref="IMessage"/>'s of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the messages to be observed. </typeparam>
    public interface IMessageStream<T> : IObservable<T> where T : IMessage
    { }
}