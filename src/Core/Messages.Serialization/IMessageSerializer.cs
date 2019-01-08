namespace CustomCode.Core.Messages.Serialization
{
    /// <summary>
    /// Interface for types that can serialize an <see cref="IMessage"/> to a binary array.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// Serialize the specified <paramref name="message"/> to a binary array.
        /// </summary>
        /// <typeparam name="T"> The type of the message to be serialized. </typeparam>
        /// <param name="message"> The message to be serialized. </param>
        /// <returns> The binary array that contains the serialized <paramref name="message"/>. </returns>
        byte[] Serialize<T>(T message) where T : IMessage;
    }
}