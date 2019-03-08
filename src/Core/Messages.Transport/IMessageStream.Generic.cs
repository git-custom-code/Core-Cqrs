namespace CustomCode.Core.Messages.Transport
{
    using System;

    /// <summary>
    /// Interface for an observable stream of filtered <see cref="IMessage"/>'s of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the filtered message instances. </typeparam>
    public interface IMessageStream<T> : IObservable<T> where T : IMessage
    { }
}