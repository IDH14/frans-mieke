using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Client_IDH14.Models
{
    public class Checksums
    {
        static string file = @"checksums.csv";
        public static void ExistsChecksums(string path)
        {
            if (Directory.Exists(path))
            {
                Console.WriteLine("That directory exists already.");
                if (!File.Exists(path + file))
                {
                    CreateChecksums(path, file);
                }
                else
                {
                    Console.WriteLine("That file exists already.");
                }
            }
            else
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory (idh14Server) was created successfully at {0}.", Directory.GetCreationTime(path));
                Checksums.ExistsChecksums(path);
            }
            
        }
        public static void CreateChecksums(string path, string file)
        {
            File.Create(path + file);
            Console.WriteLine("The file (Checksums.csv) was created successfully at {0}.", Directory.GetCreationTime(path));
        }
    }
}