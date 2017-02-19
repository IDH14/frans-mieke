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

        public static string ResponsePUT200ToJson()
        {
            string str = "RESPONSE {";
            str += " 'status': '200',";
            str += " 'message' : 'The file is added to the server' ";
            str += "}";

            return str;
        }

        public static string ResponsePUT412ToJson()
        {
            string str = "RESPONSE {";
            str += " 'status': '412',";
            str += " 'message' : 'You are trying to add an older version of a document' ";
            str += "}";

            return str;
        }

        public static string ResponseDELETE200ToJSON()
        {
            string str = "RESPONSE {";
            str += " 'status': '200',";
            str += " 'message' : 'The file is deleted from the server' ";
            str += "}";

            return str;
        }

        public static string ResponseDELETE404ToJSON()
        {
            string str = "RESPONSE {";
            str += " 'status': '404',";
            str += " 'message' : 'File not Found' ";
            str += "}";

            return str;
        }

        public static string ResponseDELETE412ToJSON()
        {
            string str = "RESPONSE {";
            str += " 'status': '412',";
            str += " 'message' : 'You are trying to delete an older version of a document' ";
            str += "}";

            return str;
        }

        public static string ResponseLIST200ToJSON(string path)
        {
            string str = "RESPONSE {";
            str += " 'status': '200',";
            str += " 'files': [";
            
            DirectoryInfo c = new DirectoryInfo(path);
            FileInfo[] Files2 = c.GetFiles("*.*");

            foreach (FileInfo file in Files2)
            {
                FileHandler tempFile = new FileHandler();
                string name = file.Name;
                string name2 = Base64.Base64Encode(name);

                string checksum = Checksums.GetSha1Hash(path + @"\" + name);

                string str1 = "{ 'filename': '" + name2 + "', 'checksum': '" + checksum + "' }";

                str += str1;
            }

            str += "] }";
            return str;
        }
    }
}
