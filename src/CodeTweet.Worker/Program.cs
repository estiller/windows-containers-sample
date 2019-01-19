using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using CodeTweet.Notifications;
using Topshelf;

namespace CodeTweet.Worker
{
    class Program
    {
        static void Main()
        {
            string rabbitMqConnectionString = System.Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING");
            if (rabbitMqConnectionString != null)
            {
                ConfigurationManager.AppSettings["RabbitMQ_ConnectionString"] = rabbitMqConnectionString;
            }

            string sqlConnectionString = System.Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (sqlConnectionString != null)
            {
                var currentConnectionString = ConfigurationManager.ConnectionStrings["CodeTweetIdentity"];

                // This is a hack. Do not use this in production code... :)
                var fi = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                Debug.Assert(fi != null);
                fi.SetValue(currentConnectionString, false);
                currentConnectionString.ConnectionString = sqlConnectionString;
            }

            HostFactory.Run(x =>
            {
                x.Service<NotificationService>(s =>
                {
                    s.ConstructUsing(name => new NotificationService());
                    s.WhenStarted(ns => ns.Start());
                    s.WhenStopped(ns => ns.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("CodeTweet Notification Service");
                x.SetDisplayName("CodeTweet");
                x.SetServiceName("CodeTweet");
            });
        }
    }
}
