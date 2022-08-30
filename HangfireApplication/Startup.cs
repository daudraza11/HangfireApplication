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
            GlobalConfiguration.Configuration.UseRedisStorage(ConfigurationManager.ConnectionStrings["HangfireRedis"].ConnectionString);
            var options = new BackgroundJobServerOptions
            {
                ServerName = "Hangfire Server"
            };
            app.UseHangfireServer(options);
            app.UseHangfireDashboard();
            
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            //if (ConfigurationManager.AppSettings["LiveEmailEnvironment"] == "0")
            //{
            app.UseHangfireServer();
            RecurringJobs.RegisterRecurringJobs(1);
            //}
        }
    }

}