using Hangfire;
using Hangfire.JobsTrigger;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HangfireApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (ConfigurationManager.AppSettings["HangfireType"] == "R")
            {
                GlobalConfiguration.Configuration.UseRedisStorage(ConfigurationManager.ConnectionStrings["HangfireRedis"].ConnectionString);
            }
            else
            {
                GlobalConfiguration.Configuration.UseSqlServerStorage("MailerDb");
            }

            app.UseHangfireDashboard();
            //LogProvider.SetCurrentLogProvider(new DummyLogProviderHangfire());
            //GlobalJobFilters.Filters.Add(new ShortExpirationTimeAttribute());
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            if (ConfigurationManager.AppSettings["LiveEmailEnvironment"] == "0")
            {
                app.UseHangfireServer();
                RecurringJobs.RegisterRecurringJobs(1);
            }
        }
    }

}