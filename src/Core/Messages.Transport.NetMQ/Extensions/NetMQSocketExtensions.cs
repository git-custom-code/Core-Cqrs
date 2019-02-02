namespace CustomCode.Core.Messages.Transport.NetMQ.Extensions
{
    using global::NetMQ;
    using System;

    /// <summary>
    /// Extension methods for the <see cref="NetMQSocket"/> type.
    /// </summary>
    public static class NetMQSocketExtensions
    {
        #region Logic

        /// <summary>
        /// Bind the extended <see cref="NetMQSocket"/> to the specified <paramref name="endpoint"/>.
        /// </summary>
        /// <param name="socket"> The socket that should be bound to the specified <paramref name="endpoint"/>. </param>
        /// <param name="endpoint"> The endpoint that should be bound to the <paramref name="socket"/>. </param>
        public static void Bind(this NetMQSocket socket, ISocketEndpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            socket.Bind(endpoint.AsString());
        }

        /// <summary>
        /// Connect the extended <see cref="NetMQSocket"/> to the specified <paramref name="endpoint"/>.
        /// </summary>
        /// <param name="socket"> The socket that should be connected to the specified <paramref name="endpoint"/>. </param>
        /// <param name="endpoint"> The endpoint that should be connected to the <paramref name="socket"/>. </param>
        public static void Connect(this NetMQSocket socket, ISocketEndpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            socket.Connect(endpoint.AsString());
        }

        #endregion
    }
}