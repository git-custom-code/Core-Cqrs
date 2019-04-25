namespace CustomCode.Core.Messages.Transport
{
    /// <summary>
    /// Interface for an observable stream of filtered <see cref="IEvent"/>'s of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the filtered event instances. </typeparam>
    public interface IEventStream<T> : IMessageStream<T> where T : IEvent
    { }
}