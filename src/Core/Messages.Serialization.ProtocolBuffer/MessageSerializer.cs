namespace CustomCode.Core.Messages.Serialization.ProtocolBuffer
{
    using ProtoBuf.Meta;
    using System.IO;

    /// <summary>
    /// An <see cref="IMessageSerializer"/> implementation using google's protocol buffer.
    /// </summary>
    public sealed class MessageSerializer : IMessageSerializer
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="MessageSerializer"/> type.
        /// </summary>
        /// <param name="model">
        /// Specify a custom <see cref="RuntimeTypeModel"/> if you need to change protocol buffer's default serialization rules.
        /// </param>
        public MessageSerializer(RuntimeTypeModel model = null)
        {
            Model = model ?? RuntimeTypeModel.Default;
        }

        /// <summary>
        /// Gets the <see cref="RuntimeTypeModel"/> that specifies protocol buffer's serialization rules.
        /// </summary>
        private RuntimeTypeModel Model { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public byte[] Serialize<T>(T message) where T : IMessage
        {
            using (var stream = new MemoryStream())
            {
                Model.Serialize(stream, message);
                return stream.ToArray();
            }
        }

        #endregion
    }
}