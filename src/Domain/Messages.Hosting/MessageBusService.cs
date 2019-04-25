namespace CustomCode.Domain.Messages.Hosting
{
    using Core.Messages.Transport;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// An <see cref="IHostedService"/> implementation that will host a sinlge <see cref="IMessageBus"/> instance.
    /// </summary>
    public sealed class MessageBusService : IHostedService, IDisposable
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="MessageBusService"/> type.
        /// </summary>
        /// <param name="messageBus"> The hosted <see cref="IMessageBus"/> instance. </param>
        public MessageBusService(IMessageBus messageBus)
        {
            MessageBus = messageBus;
        }

        /// <summary>
        /// Gets the hosted <see cref="IMessageBus"/> instance.
        /// </summary>
        private IMessageBus MessageBus { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public void Dispose()
        {
            MessageBus.Dispose();
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return MessageBus.StartAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return MessageBus.StopAsync(cancellationToken);
        }

        #endregion
    }
}