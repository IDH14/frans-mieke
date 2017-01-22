using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    public class FileHandler
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Checksum { get; set; }
        public string OriginalChecksum { get; set; }

        public static string Response200ToJSON(string fileName, string content, string entry)
        {
            string name = Base64.Base64Encode(fileName);
            string checksum = GetSha1Hash(entry);
                        
            string str = "RESPONSE {";
            str += " 'status': 200,";
            str += " 'filename': '" + name;
            str += " 'content': '" + content;
            str += " 'checksum': '" + checksum;
            str += "'}";

            return str;
        }

        public static string GetSha1Hash(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                SHA1 sha = new SHA1Managed();
                return BitConverter.ToString(sha.ComputeHash(fs));
            }
        }
    }
}
