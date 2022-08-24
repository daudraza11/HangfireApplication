using Hangfire;
using Hangfire.Business;
using Hangfire.JobsTrigger;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HangfireConsole
{
    public class Program
    {
        static string DefaultMessage = "";

        static void Main(string[] args)
        {

            var manager = new SetupManager();
            manager.InitSystemSettings();

            DefaultMessage = "Hangfire Server started. Press any key to exit...";
            if (ConfigurationManager.AppSettings["HangfireType"] == "R")
            {
                GlobalConfiguration.Configuration.UseRedisStorage(ConfigurationManager.ConnectionStrings["HangfireRedis"].ConnectionString);
            }
            else
            {
                GlobalConfiguration.Configuration.UseSqlServerStorage("MailerDb");
            }
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            var options = new BackgroundJobServerOptions
            {
                // This is the default value
                WorkerCount = Environment.ProcessorCount * Convert.ToInt32(ConfigurationManager.AppSettings["HangfireThreads"])
            };
            using (var server = new BackgroundJobServer(options))
            {
                Console.WriteLine(DefaultMessage);
                int RecurringJobsEnabled = Convert.ToInt32(ConfigurationManager.AppSettings["RecurringJobsEnabled"]);
                if (RecurringJobsEnabled == 0)
                {
                    RemoveAllRecurringJobs();
                    Console.WriteLine("Recurring Jobs Removed.");
                }
                else
                {
                    RegisterBckgroundJobs();
                    Console.WriteLine("Recurring Jobs Added.");
                }
                Console.ReadKey();
            }
        }
        static void RegisterBckgroundJobs()
        {
            try
            {
                Int32 instance = Convert.ToInt32(ConfigurationManager.AppSettings["InstanceNumber"]);
                Console.WriteLine("Adding Jobs For Instance=" + instance);
                RecurringJobs.RegisterRecurringJobs(instance);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
        static void RegisterRecurringJobs(string jobName, Expression<Action> methodCall, Func<string> cron,
          string cronExpression = "")
        {
            RecurringJob.AddOrUpdate(jobName, methodCall, cron != null ? cron() : cronExpression);
        }
        static void RemoveAllRecurringJobs()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in StorageConnectionExtensions.GetRecurringJobs(connection))
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }
        }
    }
}
