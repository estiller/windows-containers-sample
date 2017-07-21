using CodeTweet.Queueing;
using CodeTweet.Queueing.RabbitMQ;
using Ninject.Modules;

namespace CodeTweet.Web.Modules
{
    public class QueueingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<INotificationEnqueue>().To<RabbitNotificationEnqueue>().InSingletonScope();
        }
    }
}