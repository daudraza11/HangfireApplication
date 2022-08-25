using GenericDAL.GenericDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Business
{
    public class BackgroundJobConfig
    {
        public List<JobConfiguration> GetConfigurations()
        {
            return GetConfigurations(DALHelperOracle.GetDataTable("select * from job_configuration_hangfire"));
        }

        public List<JobConfiguration> GetConfigurationsUpdates()
        {
            return GetConfigurations(DALHelperOracle.GetDataTable("select * from job_configuration_hangfire where REQUIRES_UPDATE=1"));
        }

        public void ResetUpdate(string jobCode)
        {
            DALHelperOracle.Execute(string.Format("update job_configuration_hangfire set REQUIRES_UPDATE=0 where JOB_CODE='{0}'", jobCode));
        }


        private List<JobConfiguration> GetConfigurations(DataTable dt)
        {
            var lst = new List<JobConfiguration>();
            foreach (DataRow row in dt.Rows)
            {
                lst.Add(ToJobConfiguration(row));
            }
            return lst;
        }

        JobConfiguration ToJobConfiguration(DataRow row)
        {
            return new JobConfiguration()
            {
                CronExpression = Convert.ToString(row["CRON_EXPRESSION"]),
                ScheduleType = Convert.ToString(row["SCHEDULE_TYPE"]),
                Enabled = Convert.ToInt32(row["ENABLED"]) == 1,
                JobCode = Convert.ToString(row["JOB_CODE"]),
                JobDescription = Convert.ToString(row["JOB_DESC"]),
                UpdateRequired = Convert.ToInt32(row["REQUIRES_UPDATE"]) == 1,
                InstanceNumber = Convert.ToInt32(row["INSTANCE_NUMBER"])
            };
        }
    }

    public class JobCodes
    {
        public const string RefreshRecurringJobs = "REFRESH_RECURRING_JOBS";
        public const string UsisApi = "USIS_API";
        public const string PushInterpayTransactions = "PUSH_INTERPAY_TRANSACTIONS";
        public const string PushDirectDebitTransactions = "PUSH_DIRECT_DEBIT_TRANSACTIONS";
        public const string PushAccessBankTransfer = "PUSH_ACCESS_BANK_TRANSFERS"; 
        public const string GenerateBIMAReportFiles = "GENERATE_BIMA_REPORT_FILES"; 

    }

    public class JobConfiguration
    {
        public string JobCode { get; set; }

        public string JobDescription { get; set; }

        public string ScheduleType { get; set; }

        public string CronExpression { get; set; }

        public bool Enabled { get; set; }
        public bool UpdateRequired { get; set; }
        public Int32 InstanceNumber { get; set; }

    }
}
