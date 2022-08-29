using Renci.SshNet;
using System;
using System.Collections.Generic;
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
            var host = "192.168.30.47";
            var port = 2222;
            var username = "tester";
            var password = "password";

            SftpClient client = new SftpClient(host, port, username, password);
            client.Connect();
            
            string localDirectory = HostingEnvironment.MapPath(("~/Contents/BIMAFile/"));

            string[] files = Directory.GetFiles(localDirectory);
            foreach (string file in files)
            {
                using (Stream inputStream = new FileStream(file,FileMode.Open))
                {
                    client.UploadFile(inputStream, Path.GetFileName(file));
                }
            }
        }
    }
}
