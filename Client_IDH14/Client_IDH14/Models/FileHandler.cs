using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Client_IDH14.Models
{
    public class FileHandler
    {
        public string FileName { get; set; }
        public string Checksum { get; set; }
        public string OriginalChecksum { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Files { get; set; }

        public static List<FileHandler> GetFiles()
        {
            var model = new List<FileHandler>();

            // Set c so folder can be checked by client
            DirectoryInfo c = new DirectoryInfo(@"C:\idh14Client\");

            //Get all files
            FileInfo[] Files2 = c.GetFiles("*.*");

            foreach (FileInfo file in Files2)
            {
                FileHandler tempFile = new FileHandler();
                tempFile.FileName = file.Name;

                string filePath = c + file.Name;

                //Show SHA1 hash of current version of the file
                tempFile.Checksum = Checksums.GetSha1Hash(filePath);

                model.Add(tempFile);
            }
            return model;
        }

        public static List<string> GetFilenames(){

            var model = new List<string>();

            // Set c so folder can be checked by client
            DirectoryInfo c = new DirectoryInfo(@"C:\idh14Client\");

            //Get all files
            FileInfo[] Files2 = c.GetFiles("*.*");

            foreach (FileInfo file in Files2)
            {
                FileHandler tempFile = new FileHandler();
                tempFile.FileName = file.Name;
                model.Add(file.Name);
            }

            return model;
        }

        public static string FileNameToJSON(string selectedFile)
        {
            string fileName = Base64.Base64Encode(selectedFile);

            string str = "GET {";
            str += " 'filename': '" + fileName;
            str += "'}";
            return str;
        }

        public static string FileToJSON(string selectedFile)
        {
            string fileName = Base64.Base64Encode(selectedFile);

            string path = "C:/idh14Client/" + selectedFile;
            Byte[] bytes = File.ReadAllBytes(path);
            String content = Convert.ToBase64String(bytes);

            string checksum = Checksums.GetSha1Hash(path);
            string originalchecksum = "test";

            string str = "PUT {";
            str += " 'filename': '" + fileName + "',";
            str += " 'content': '" + content + "',";
            str += " 'checksum': '" + checksum + "',";
            str += " 'originalchecksum': '" + originalchecksum + "'";
            str += "}";
            return str;
        }

        public static string ListToJSON()
        {
            string str = "LIST { }";
            return str;
        }

        public static string DeleteToJSON(string selectedFile, string checksumFile)
        {
            string fileName = Base64.Base64Encode(selectedFile);
            
            string str = "DELETE {";
            str += " 'filename': '" + fileName + "',";
            str += " 'checksum': '" + checksumFile;
            str += "'}";
            return str;
        }
    }

    public class MyClassMap : CsvClassMap<FileHandler>
    {
        public MyClassMap()
        {
            Map(m => m.FileName).Name("FileName");
            Map(m => m.Checksum).Name("Checksum");
            Map(m => m.OriginalChecksum).Name("OriginalChecksum");
        }
    }
}
