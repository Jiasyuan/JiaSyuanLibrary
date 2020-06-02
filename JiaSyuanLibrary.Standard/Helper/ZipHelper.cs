using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace JiaSyuanLibrary.Standard.Helper
{
    public static class ZipHelper
    {
        /// <summary>
        /// Zip Files
        /// </summary>
        /// <param name="filesPath">Zip file path</param>
        /// <param name="zipPath">Zip save path</param>
        /// <param name="zipPassword">Zip Password</param>
        public static void ZipFiles(IEnumerable<string> filesPath, string zipPath, string zipPassword = "")
        {
            using (ZipFile zip = new ZipFile())
            {
                if (!string.IsNullOrEmpty(zipPassword))
                    zip.Password = zipPassword;
                if (filesPath != null && filesPath.Any())
                {
                    foreach (var filePath in filesPath)
                    {
                        zip.AddFile(filePath, "");
                    }
                    zip.Save(zipPath);
                }
            }
        }

        /// <summary>
        /// UnZip Files
        /// </summary>
        /// <param name="zipPath">Zip path</param>
        /// <param name="outputPath">Extract path</param>
        /// <param name="zipPassword">Zip Password</param>
        public static void UnZipFiles(string zipPath, string outputPath, string zipPassword = "")
        {
            if (File.Exists(zipPath))
            {
                var options = new ReadOptions { StatusMessageWriter = System.Console.Out };
                using (ZipFile zip = ZipFile.Read(zipPath, options))
                {
                    if (!string.IsNullOrEmpty(zipPassword))
                        zip.Password = zipPassword;
                    zip.ExtractAll(outputPath);
                }

            }
        }
    }
}
