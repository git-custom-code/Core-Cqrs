namespace CustomCode.Core.Messages.Transport
{
    /// <summary>
    /// Interface for an observable stream of <see cref="IEvent"/>'s.
    /// </summary>
    public interface IEventStream : IMessageStream<IEvent>
    { }
}