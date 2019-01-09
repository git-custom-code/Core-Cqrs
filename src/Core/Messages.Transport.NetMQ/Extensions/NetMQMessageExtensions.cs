namespace CustomCode.Core.Messages.Transport.NetMQ.Extensions
{
    using global::NetMQ;
    using System;

    /// <summary>
    /// Extension methods for the <see cref="NetMQMessage"/> type.
    /// </summary>
    public static class NetMQMessageExtensions
    {
        #region Logic

        /// <summary>
        /// Get the type of the serialized <see cref="IMessage"/>.
        /// </summary>
        /// <param name="message"> The exended netMQ message. </param>
        /// <returns> The type of the serialized <see cref="IMessage"/>. </returns>
        public static string MessageType(this NetMQMessage message)
        {
            if (message.FrameCount != 2)
            {
                throw new ArgumentException("Invalid number of frames", nameof(message));
            }

            return message.First.ConvertToString();
        }

        /// <summary>
        /// Gets the raw binary data of the serialized <see cref="IMessage"/>.
        /// </summary>
        /// <param name="message"> The exended netMQ message. </param>
        /// <returns> The serialized <see cref="IMessage"/> as raw binary data. </returns>
        public static byte[] SerializedMessage(this NetMQMessage message)
        {
            if (message.FrameCount != 2)
            {
                throw new ArgumentException("Invalid number of frames", nameof(message));
            }

            return message.Last.Buffer;
        }

        #endregion
    }
}