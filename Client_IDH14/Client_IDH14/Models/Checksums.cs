using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Client_IDH14.Models
{
    public class Checksums
    {
        static string file = @"checksums.csv";

        public static void ExistsChecksums(string path)
        {
            if (Directory.Exists(path))
            {
                if (!File.Exists(path + file))
                {
                    CreateChecksumsFile(path, file);
                }
            }
            else
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Checksums.ExistsChecksums(path);
            }
        }

        public static void CreateChecksumsFile(string path, string file)
        {
            var checksumFile = File.Create(path + file);
            checksumFile.Close();
        }

        public static void UpdateNewChecksums(string path)
        {
            using (var writer = new StreamWriter(path + file))
            {
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteHeader<FileHandler>();
                    var list = FileHandler.GetFiles();

                    foreach (var item in list)
                    {
                        item.OriginalChecksum = item.Checksum;

                        csv.WriteField(item.FileName);
                        csv.WriteField(item.Checksum);
                        csv.WriteField(item.OriginalChecksum);
                        csv.NextRecord();
                    }
                }
            }
        }
        public static string getChecksumFile(string path, string checksumPath, string fileName)
        {
            string checksum = "";
            string[] existingLines = File.ReadAllLines(path + checksumPath + file);
            existingLines = existingLines.Skip(1).ToArray();

            List<string> newLines = new List<string>();

            foreach (string line in existingLines)
            {
                string[] columns = line.Split(new char[] { ',' });
                checksum = columns[1];
                string name = columns[0];
                if (fileName == name)
                {
                    return checksum;
                }
            }
            return checksum;
        }
        public static void deleteChecksumFromFile(string path, string checksumPath, string fileName)
        {
            string checksum = "";
            string[] existingLines = File.ReadAllLines(path + checksumPath + file);
            //existingLines = existingLines.Skip(1).ToArray();

            List<string> newLines = new List<string>();

            foreach (string line in existingLines)
            {
                string[] columns = line.Split(new char[] { ',' });
                checksum = columns[1];
                string name = columns[0];
                if (fileName != name)
                {
                    newLines.Add(string.Join(",", columns));
                }
            }
        }

        public static bool checksumFileCorrect(string path, string checksumPath, string fileName) {
            string[] existingLines = File.ReadAllLines(path + checksumPath + file);
            existingLines = existingLines.Skip(1).ToArray();

            List<string> newLines = new List<string>();

            foreach (string line in existingLines)
            {
                string[] columns = line.Split(new char[] { ',' });
                string checksum = columns[1];
                string name = columns[0];
                if (fileName == name) {
                    string checksum1 = GetSha1Hash(path + name);
                    if (checksum == checksum1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void UpdateChecksums(string path, string newChecksum, string fileName)
        {

            string[] existingLines = File.ReadAllLines(path + file);
            //existingLines = existingLines.Skip(1).ToArray();

            List<string> newLines = new List<string>();

            foreach (string line in existingLines)
            {

                string[] columns = line.Split(new char[] { ',' });
                string checksum = columns[1];
                string name = columns[0];
                string newLine;
                if (fileName == name)
                {
                    columns[1] = newChecksum;
                    newLine = string.Join(",", columns);
                    newLines.Add(newLine);
                }else { 

                /*
                string checksum1 = GetSha1Hash(path + name);

                if(checksum != checksum1)
                    {
                        checksum = checksum1;
                    }

                columns[1] = checksum;
                string newLine = string.Join(",", columns);
                newLines.Add(newLine);
                */
                columns[1] = checksum;
                newLine = string.Join(",", columns);
                newLines.Add(newLine);
                }
            }

            File.WriteAllLines(path + file, newLines);
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