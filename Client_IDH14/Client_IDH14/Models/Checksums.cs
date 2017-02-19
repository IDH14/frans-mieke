using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                    CreateChecksums(path, file);
                    UpdateNewChecksums(path);
                }
                else
                {
                    UpdateChecksums(path);
                }
            }
            else
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Checksums.ExistsChecksums(path);
            }
        }

        public static void CreateChecksums(string path, string file)
        {
            var checksumFile = File.Create(path + file);
            checksumFile.Close();
        }

        public static void UpdateNewChecksums(string path)
        {
            string checksumFile = @"checksums.csv";

            using (var writer = new StreamWriter(path + checksumFile))
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

        public static void UpdateChecksums(string path)
        {
            string checksumFile = @"checksums.csv";

            string[] existingLines = File.ReadAllLines(path + checksumFile);
            existingLines = existingLines.Skip(1).ToArray();

            List<string> newLines = new List<string>();

            foreach (string line in existingLines)
            {
                string[] columns = line.Split(new char[] { ',' });
                string checksum = columns[1];
                string name = columns[0];

                string checksum1 = FileHandler.GetSha1Hash(path + name);

                if(checksum != checksum1)
                    {
                        checksum = checksum1;
                    }

                columns[1] = checksum;
                string newLine = string.Join(",", columns);
                newLines.Add(newLine);           
            }

            File.WriteAllLines(path + checksumFile, newLines);
        }
    }
}