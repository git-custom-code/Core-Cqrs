namespace CustomCode.Core.Messages.Transport
{
    /// <summary>
    /// Interface for an observable stream of filtered <see cref="ICommand"/>'s of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the filtered command instances. </typeparam>
    public interface ICommandStream<T> : IMessageStream<T> where T : ICommand
    { }
}