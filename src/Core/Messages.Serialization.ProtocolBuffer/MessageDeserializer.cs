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
                return (T)Model.Deserialize(stream, null, typeof(T));
            }
        }

        #endregion
    }
}