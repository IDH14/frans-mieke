using System;
using System.IO;
using System.Security.Cryptography;

namespace TcpServer
{
    public class FileHandler
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Checksum { get; set; }
        public string OriginalChecksum { get; set; }

        public static string ResponseGET200ToJSON(string name2, string content, string entry)
        {
            string name = Base64.Base64Encode(name2);
            string checksum = GetSha1Hash(entry);
                        
            string str = "RESPONSE {";
            str += " 'status': '200',";
            str += " 'filename': '" + name + "',";
            str += " 'content': '" + content + "',";
            str += " 'checksum': '" + checksum;
            str += "'}";

            return str;
        }

        public static string ResponseGET404ToJSON()
        {
            string str = "RESPONSE {";
            str += " 'status': '404'";
            str += "}";

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
