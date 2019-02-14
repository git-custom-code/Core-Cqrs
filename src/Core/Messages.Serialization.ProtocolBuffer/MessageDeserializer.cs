namespace CustomCode.Core.Messages.Serialization.ProtocolBuffer
{
    using ProtoBuf.Meta;
    using System.IO;

    /// <summary>
    /// An <see cref="IMessageDeserializer"/> implementation using google's protocol buffer.
    /// </summary
    public sealed class MessageDeserializer : IMessageDeserializer
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="MessageDeserializer"/> type.
        /// </summary>
        /// <param name="model">
        /// Specify a custom <see cref="RuntimeTypeModel"/> if you need to change protocol buffer's default serialization rules.
        /// </param>
        public MessageDeserializer(RuntimeTypeModel model = null)
        {
            Model = model ?? RuntimeTypeModel.Default;
            if (!Model.IsDefined(typeof(MessageEnvelope)))
            {
                Model.Add(typeof(MessageEnvelope), true);
            }
        }

        /// <summary>
        /// Gets the <see cref="RuntimeTypeModel"/> that specifies protocol buffer's serialization rules.
        /// </summary>
        private RuntimeTypeModel Model { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public T Deserialize<T>(byte[] data) where T : IMessage
        {
            using (var stream = new MemoryStream(data))
            {
                var envelope = (MessageEnvelope)Model.Deserialize(stream, null, typeof(MessageEnvelope));
                using (var messageStream = new MemoryStream(envelope.SerializedMessage))
                {
                    if (!Model.IsDefined(envelope.Type))
                    {
                        Model.Add(envelope.Type, true);
                    }

                    var message = (T)Model.Deserialize(messageStream, null, envelope.Type);
                    return message;
                }
            }
        }

        #endregion
    }
}
