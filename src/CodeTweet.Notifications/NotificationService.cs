using System;
using CodeTweet.DomainModel;
using CodeTweet.IdentityDal;
using CodeTweet.Queueing;
using CodeTweet.Queueing.RabbitMQ;
using NLog;
using Polly;

namespace CodeTweet.Notifications
{
    public class NotificationService
    {
        private readonly INotificationDequeue _dequeue = new RabbitNotificationDequeue();
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private IDisposable _subscription;

        public void Start()
        {
            var retryPolicy = Policy
                .Handle<TimeoutException>()
                .RetryForever(exception =>
                {
                    _logger.Warn(exception, "A timeout occured while trying to subscribe");
                });
            retryPolicy.Execute(() =>
            {
                _subscription = _dequeue.Subscribe(OnTweet);
            });
            _logger.Info("Subscription started");
        }

        public void Stop()
        {
            _subscription.Dispose();
            _dequeue.Dispose();
        }

        // 'async void' is usually not a good idea, but it is outside the scope of this sample
        private async void OnTweet(Tweet tweet)
        {
            try
            {
                using (var context = new ApplicationIdentityContext())
                {
                    var repository = new UserRepository(context);
                    var users = await repository.GetUsersWithNotificationsAsync(); // Can be cached
                    foreach (var user in users)
                    {
                        SendNotification(tweet, user);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occured while processing recieved tweets");
            }
        }

        private void SendNotification(Tweet tweet, string user)
        {
            _logger.Info($"Sent notification to user '{user}'. Tweet text: '{tweet.Text}'");
        }
    }
}