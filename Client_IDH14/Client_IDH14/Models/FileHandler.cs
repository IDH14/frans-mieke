using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Client_IDH14.Models
{
    public class FileHandler
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Checksum { get; set; }
        public string OriginalChecksum { get; set; }

        public static string GetSha1Hash(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                SHA1 sha = new SHA1Managed();
                return BitConverter.ToString(sha.ComputeHash(fs));
            }
        }

        public static void UpdateChecksums(string folderPath)
        {
            String checksumFile = @"\checksums.csv";

            if (!File.Exists(folderPath + checksumFile))
            {
                File.Create(folderPath + checksumFile);
            }

            foreach (var file in folderPath)
            {

            }
         

        }
    }
}