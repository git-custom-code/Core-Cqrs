namespace CustomCode.Core.Messages.Serialization.ProtocolBuffer
{
    using System;

    /// <summary>
    /// An envelope that adds type information for a serialized <see cref="IMessage"/> to the .proto in order
    /// for better deserialization support.
    /// </summary>
    public sealed class MessageEnvelope
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="MessageEnvelope"/> type.
        /// </summary>
        /// <param name="type"> The type of the contained serialized <see cref="IMessage"/>. </param>
        /// <param name="serializedMessage"> The serialized <see cref="IMessage"/>. </param>
        public MessageEnvelope(Type type, byte[] serializedMessage)
        {
            Type = type;
            SerializedMessage = serializedMessage;
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets the type of the contained serialized <see cref="IMessage"/>.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the serialized <see cref="IMessage"/>.
        /// </summary>
        public byte[] SerializedMessage { get; }

        #endregion
    }
}
