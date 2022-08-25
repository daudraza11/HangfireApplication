using Common;
using Hangfire.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Business
{
    public class ReportBE
    {
        public byte[] GetCustomersV1Data()
        {
            var dt = new ReportDAL().GetCustomersV1Data();
            string FileName = "Cus1_" + DateTime.Now.ToString(("yyyy-dd-M"));
            string fileExtension = ".csv";
            string fileWithExtension = FileName + fileExtension;

            string PathOfFile = System.Web.HttpContext.Current.Server.MapPath(("~/Contents/Cus1/")) + FileName;
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath(("~/Contents/Cus1/"));

            new ZipWriterManager().FileDirectoryPath(directoryPath, FileName);
            var zipBytes = GenericExportFile(directoryPath, FileName, PathOfFile, dt);
            return zipBytes;
        }

        public byte[] GetCustomersV3Data()
        {
            var dt = new ReportDAL().GetCustomersV3Data();
            string FileName = "Cus3_" + DateTime.Now.ToString(("yyyy-dd-M"));
            string fileExtension = ".csv";
            string fileWithExtension = FileName + fileExtension;

            string PathOfFile = System.Web.HttpContext.Current.Server.MapPath(("~/Contents/Cus3/")) + FileName;
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath(("~/Contents/Cus3/"));
    
            var zipBytes = GenericExportFile(directoryPath, FileName, PathOfFile, dt);
            return zipBytes;
        }

        public byte[] GetCustomersV4Data()
        {
            var dt = new ReportDAL().GetCustomersV4Data();
            string FileName = "Cus4_" + DateTime.Now.ToString(("yyyy-dd-M"));
            string fileExtension = ".csv";
            string fileWithExtension = FileName + fileExtension;

            string PathOfFile = System.Web.HttpContext.Current.Server.MapPath(("~/Contents/Cus4/")) + FileName;
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath(("~/Contents/Cus4/"));

            var zipBytes = GenericExportFile(directoryPath, FileName, PathOfFile, dt);
            return zipBytes;
        }

        public byte[] GenericExportFile(string directoryPath, string FileName, string PathOfFile, DataTable dt)
        {
            new ZipWriterManager().FileDirectoryPath(directoryPath, FileName);

            System.IO.DirectoryInfo di = new DirectoryInfo(directoryPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            StringBuilder sb = new StringBuilder();

            if (!System.IO.File.Exists(PathOfFile))
            {
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));
            }
            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            System.IO.File.AppendAllText(PathOfFile, sb.ToString());
            List<string> filesList = new List<string>();
            filesList.Add(PathOfFile);

            new ZipWriterManager().CreateDocumentationZipFile(FileName, directoryPath, filesList);

            byte[] bytes = System.IO.File.ReadAllBytes(PathOfFile); //create file into folder

            var zipBytes = new ZipWriterManager().ConvertByte_For_ZipDownload(PathOfFile);

            if (System.IO.File.Exists(Path.Combine(directoryPath, FileName)))
            {
                System.IO.File.Delete(Path.Combine(directoryPath, FileName)); //delete created which is in folder
            }
            return zipBytes;

        }

    }
}
