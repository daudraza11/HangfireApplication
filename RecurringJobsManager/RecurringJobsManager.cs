using Hangfire.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Jobs
{
    public class RecurringJobsManager
    {
        public void PushDirectDebitTransactions()
        {
            //var jobs = new InterpayAfricaJobs();
            //jobs.PushDirectDebitTransactions();
        }
        public void GenerateBimaFiles()
        {
            try
            {
                new ReportBE().GetCustomersV1Data();
                new ReportBE().GetCustomersV3Data();
                new ReportBE().GetCustomersV4Data();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
