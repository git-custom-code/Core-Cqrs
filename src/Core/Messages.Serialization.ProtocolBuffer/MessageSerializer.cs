namespace CustomCode.Core.Messages.Serialization.ProtocolBuffer
{
    using ProtoBuf.Meta;
    using System.IO;
    using System.IO.Compression;

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
        public byte[] Serialize<T>(T message) where T : IMessage
        {
            var messageType = typeof(T);
            using (var messageStream = new MemoryStream())
            {
                if (!Model.IsDefined(messageType))
                {
                    Model.Add(messageType, true);
                }

                Model.Serialize(messageStream, message);
                var serializedMessage = messageStream.ToArray();

                using (var stream = new MemoryStream())
                {
                    var envelope = new MessageEnvelope(messageType, serializedMessage);
                    Model.Serialize(stream, envelope);
                    return stream.ToArray();
                }
            }
        }

        public byte[] SerializeCompress<T>(T message) where T : IMessage
        {
            var messageType = typeof(T);
            using (var messageStream = new MemoryStream())
            {
                if (!Model.IsDefined(messageType))
                {
                    Model.Add(messageType, true);
                }

                Model.Serialize(messageStream, message);
                var serializedMessage = messageStream.ToArray();

                using (var stream = new MemoryStream())
                {
                    var envelope = new MessageEnvelope(messageType, serializedMessage);
                    Model.Serialize(stream, envelope);

                    using (var compressedStream = new MemoryStream())
                    {
                        stream.Position = 0;
                        using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                        {
                            stream.CopyTo(gzipStream);
                        }
                        return compressedStream.ToArray();
                    }
                }
            }
        }

        #endregion
    }
}
