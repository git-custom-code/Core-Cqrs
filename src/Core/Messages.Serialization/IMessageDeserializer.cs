namespace CustomCode.Core.Messages.Serialization
{
    /// <summary>
    /// Interface for types that can deserialize an <see cref="IMessage"/> from a binary array.
    /// </summary>
    public interface IMessageDeserializer
    {
        /// <summary>
        /// Deserialize a message of type <typeparamref name="T"/> from a binary array.
        /// </summary>
        /// <typeparam name="T"> The type of the message to be deserialized. </typeparam>
        /// <param name="serializedMessage"> The serialized message to be deserialized. </param>
        /// <returns> The deserialized message. </returns>
        T Deserialize<T>(byte[] serializedMessage) where T : IMessage;
    }
}