namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using global::NetMQ;
    using System;
    using System.Reactive.Disposables;

    public sealed class MessageSource : IMessageSource
    {
        public IDisposable Subscribe(IObserver<NetMQMessage> observer)
        {
            var dummy = new NetMQMessage();
            observer.OnNext(dummy);
            return Disposable.Empty;
        }
    }
}