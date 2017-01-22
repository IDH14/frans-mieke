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
        public string Content { get; set; }
        public string Checksum { get; set; }
        public string OriginalChecksum { get; set; }
        public int Status { get; set; }

        public static string GetSha1Hash(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                SHA1 sha = new SHA1Managed();
                return BitConverter.ToString(sha.ComputeHash(fs));
            }
        }

        public static void UpdateChecksums()
        {
            string path = @"C:\";
            string checksumFile = @"checksums.csv";

            if (!File.Exists(path + checksumFile))
            {
                using (var myFile = File.Create(path + checksumFile))
                { }
            }

            if (File.Exists(path + checksumFile))
            {
                using (var writer = new StreamWriter(path + checksumFile))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<FileHandler>();
                        var list = GetFiles();

                        foreach (var file in list)
                        {
                            csv.WriteRecord(file);
                        }
                    }
                }

            }
            else
            {
                //to do: Controleren of files uit map nieuw zijn t.o.v. checksums.csv
                //If yes: informatie bijvoegen
                //If no: informatie vergelijken en updaten
            }
        }

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
                tempFile.Checksum = FileHandler.GetSha1Hash(filePath);

                model.Add(tempFile);
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

        public static string FileToJSON(string selectedFile) {

            string str = "PUT {";
            return str;
        }

        public static string ListToJSON()
        {
            string str = "LIST { }";
            return str;
        }
    }

    public class MyClassMap : CsvClassMap<FileHandler>
    {
        public MyClassMap()
        {
            Map(m => m.FileName).Name("FileName");
            Map(m => m.Content).Name("Content");
            Map(m => m.Checksum).Name("Checksum");
            Map(m => m.OriginalChecksum).Name("OriginalChecksum");
        }
    }
}
