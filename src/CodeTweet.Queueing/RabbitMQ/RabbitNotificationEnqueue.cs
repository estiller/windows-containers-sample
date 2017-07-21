using System;
using System.Configuration;
using System.Threading.Tasks;
using CodeTweet.DomainModel;
using EasyNetQ;

namespace CodeTweet.Queueing.RabbitMQ
{
    public sealed class RabbitNotificationEnqueue : INotificationEnqueue, IDisposable
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["RabbitMQ_ConnectionString"];

        private readonly IBus _bus;

        public RabbitNotificationEnqueue()
        {
            _bus = RabbitHutch.CreateBus(ConnectionString);
        }

        public Task EnqueueNotificationAsync(Tweet tweet)
        {
            return _bus.PublishAsync(tweet);
        }

        public void Dispose()
        {
            _bus.Dispose();
        }
    }
}