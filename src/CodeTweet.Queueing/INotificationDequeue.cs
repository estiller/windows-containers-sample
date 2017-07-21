using System;
using CodeTweet.DomainModel;

namespace CodeTweet.Queueing
{
    public interface INotificationDequeue : IDisposable
    {
        IDisposable Subscribe(Action<Tweet> onTweet);
    }
}