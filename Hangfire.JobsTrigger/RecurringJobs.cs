using Hangfire;
using Hangfire.Business;
using Hangfire.Jobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.JobsTrigger
{
    public class RecurringJobs
    {
        static readonly Dictionary<string, Expression<Action>> RegisteredJobs = new Dictionary<string, Expression<Action>>()
        {
            {JobCodes.GenerateBIMAReportFiles,()=>new RecurringJobsManager().GenerateBimaFiles() }
        };

        public static void RegisterRecurringJobs(int instanceNumber = -1)
        {
            var jobConfigs = new BackgroundJobConfig().GetConfigurations() ?? new List<JobConfiguration>();
            foreach (var registeredJob in RegisteredJobs)
                RegisterOrRemove(jobConfigs, registeredJob.Key, instanceNumber);
        }

        static void RegisterOrRemove(List<JobConfiguration> jobConfigurations, string jobName, Int32 instance)
        {
            var config = jobConfigurations.Find(_ => _.JobCode == jobName);
            if (config == null || config.InstanceNumber != instance)
            {
                RecurringJob.RemoveIfExists(jobName);
                return;
            }
            RegisterOrRemove(config);
        }

        public static void RegisterOrRemove(JobConfiguration config)
        {
            if (!RegisteredJobs.ContainsKey(config.JobCode))
                return;
            if (!config.Enabled)
            {
                RecurringJob.RemoveIfExists(config.JobCode);
                return;
            }
            RegisterRecurringJobs(config.JobCode, RegisteredJobs[config.JobCode],
                GetDefaultCronSchedule(config.ScheduleType), config.CronExpression);
        }


        static void RegisterRecurringJobs(string jobName, Expression<Action> methodCall, Func<string> cron,
            string cronExpression = "")
        {
            RecurringJob.AddOrUpdate(jobName, methodCall, cron != null ? cron() : cronExpression);
        }

        static Func<string> GetDefaultCronSchedule(string scheduleType)
        {
            switch (scheduleType.ToLower())
            {
                case "hourly":
                    return Cron.Hourly;
                case "daily":
                    return Cron.Daily;
                default:
                    return null;
            }
        }

        public void RefreshRecurringJobs()
        {
            Int32 instance = Convert.ToInt32(ConfigurationManager.AppSettings["InstanceNumber"]);
            var configManager = new BackgroundJobConfig();
            var jobConfigs = configManager.GetConfigurationsUpdates() ?? new List<JobConfiguration>();
            var jobConfigsall = new BackgroundJobConfig().GetConfigurations() ?? new List<JobConfiguration>();

            foreach (var config in jobConfigs)
            {
                RegisterOrRemove(jobConfigsall, config.JobCode, instance);
                if (config.InstanceNumber == instance)
                    configManager.ResetUpdate(config.JobCode);
            }
        }
    }
}
