namespace CustomCode.Core.Messages.Transport
{
    /// <summary>
    /// Interface for an observable stream of <see cref="ICommand"/>'s.
    /// </summary>
    public interface ICommandStream : IMessageStream<ICommand>
    { }
}