using System;
using System.Configuration;
using CodeTweet.DomainModel;
using EasyNetQ;

namespace CodeTweet.Queueing.RabbitMQ
{
    public sealed class RabbitNotificationDequeue : INotificationDequeue
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["RabbitMQ_ConnectionString"];

        private readonly IBus _bus;

        public RabbitNotificationDequeue()
        {
            _bus = RabbitHutch.CreateBus(ConnectionString);
        }

        public IDisposable Subscribe(Action<Tweet> onTweet)
        {
            return _bus.Subscribe("MySubscription", onTweet);
        }

        public void Dispose()
        {
            _bus.Dispose();
        }
    }
}