using Common;
using Hangfire.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Hangfire.Jobs
{
    public class RecurringJobsManager
    {

        public void GenerateBimaFiles()
        {
            try
            {
                new ReportBE().GetCustomersV1Data();
                new ReportBE().GetCustomersV3Data();
                new ReportBE().GetCustomersV4Data();
                string localDirectory = HostingEnvironment.MapPath(("~/Contents/BIMAFile/"));

                string[] files = Directory.GetFiles(localDirectory);
                if (files.Length > 0)
                {
                    new FileUploadSFTP().FileUploadSFTPServer();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogAPIException(ApiName, FuncName, response, e, TransID, TransRef);

                //Logger.LogErrorMessage(ex.Message,eErrorLogTypes.SATFFileUpload,"Hangfire BIMA",eLogCallSource.hangfireBIMASatf);
                //Logger.LogException(ex);
                throw ex;
            }
        }

    }
}
