using System;
using System.IO;

namespace TcpServer
{
    public class FileHandler
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Checksum { get; set; }
        public string OriginalChecksum { get; set; }

        public static string ResponseGET200ToJSON(string fileName, string filePath)
        {
            Byte[] bytes = File.ReadAllBytes(filePath);
            String content = Convert.ToBase64String(bytes);

            string name = Base64.Base64Encode(fileName);
            string checksum = Checksums.GetSha1Hash(filePath);

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

        public static string ResponseDELETE200ToJSON()
        {
            string str = "RESPONSE {";
            str += " 'status': '200'";
            str += " 'message' : 'The file is deleted from the server'";
            str += "}";

            return str;
        }

        public static string ResponseDELETE404ToJSON()
        {
            string str = "RESPONSE {";
            str += " 'status': '404'";
            str += " 'message' : 'File not Found'";
            str += "}";

            return str;
        }

        public static string ResponseDELETE412ToJSON()
        {
            string str = "RESPONSE {";
            str += " 'status': '412'";
            str += " 'message' : 'You are trying to delete an older version of a document'";
            str += "}";

            return str;
        }

    }
}
