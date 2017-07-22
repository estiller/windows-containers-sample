using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Owin;

namespace CodeTweet.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LoadEnvironmentConfig();
            ConfigureAuth(app);
        }

        private void LoadEnvironmentConfig()
        {
            string sqlConnectionString = System.Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (sqlConnectionString != null)
            {
                var codeTweetConnectionString = ConfigurationManager.ConnectionStrings["CodeTweet"];
                SetConnectionString(codeTweetConnectionString, sqlConnectionString);

                var identityConnectionString = ConfigurationManager.ConnectionStrings["CodeTweetIdentity"];
                SetConnectionString(identityConnectionString, sqlConnectionString);
            }
        }

        private static void SetConnectionString(ConnectionStringSettings connectionString, string sqlConnectionString)
        {
            // This is a hack. Do not use this in production code... :)
            var fi = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            Debug.Assert(fi != null);
            fi.SetValue(connectionString, false);
            connectionString.ConnectionString = sqlConnectionString;
        }
    }
}
