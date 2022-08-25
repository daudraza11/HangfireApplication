using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ZipWriterManager
    {
        public bool CreateDocumentationZipFile(string pZipFileName, string pZipDestinationPath, IList<string> pDocumentsPath)
        {
            bool zipped = false;
            try
            {
                if (pDocumentsPath.Count > 0)
                {
                    using (ZipFile loanZip = new ZipFile())
                    {
                        loanZip.AddFiles(pDocumentsPath, false, "");
                        loanZip.ParallelDeflateThreshold = -1;
                        loanZip.Save(string.Format("{0}{1}.zip", pZipDestinationPath, pZipFileName));
                        zipped = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception occur in generating Zip file. Most likely actual files not found" + ex.Message);
            }
            return zipped;
        }
        public byte[] ConvertByte_For_ZipDownload(string path)
        {


            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            byte[] byteArray = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(byteArray, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();

            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {

                    var zipEntry = archive.CreateEntry("Customer.csv",
                        CompressionLevel.Fastest);
                    using (var zipStream = zipEntry.Open())
                    {
                        zipStream.Write(byteArray, 0, byteArray.Length);
                    }

                }
                return ms.ToArray();
            }
        }
        public void FileDirectoryPath(string pFolderPath, string pFileName)
        {
            if (!Directory.Exists(pFolderPath))
            {
                Directory.CreateDirectory(pFolderPath);
            }
        }
    }

}

