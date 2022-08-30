using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Common
{
    public class FileUploadSFTP
    {
        public void FileUploadSFTPServer()
        {
            var host = ConfigurationManager.AppSettings["Host"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];

            SftpClient client = new SftpClient(host, port, username, password);
            client.Connect();

            string localDirectory = HostingEnvironment.MapPath(("~/Contents/BIMAFile/"));

            string[] files = Directory.GetFiles(localDirectory);
            foreach (string file in files)
            {
                using (Stream inputStream = new FileStream(file, FileMode.Open))
                {
                    client.UploadFile(inputStream, Path.GetFileName(file));
                }
            }
        }
    }
}
