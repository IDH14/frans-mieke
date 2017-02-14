using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client_IDH14.Models
{
    public class ServerFolder
    {
        public static void CreateFolder(string path)
        {
            // Specify the directory you want to manipulate.  
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    //Checksums.ExistsChecksums(path);
                    Console.WriteLine("That directory exists already.");
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    Console.WriteLine("The directory (idh14Server) was created successfully at {0}.", Directory.GetCreationTime(path));
                    //Checksums.ExistsChecksums(path);
                }
                // Delete the directory.
                // di.Delete();
                // Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }
        public static void CreateFolderChecksum(string path, string pathChecksum)
        {
            // Specify the directory you want to manipulate.  
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path + pathChecksum))
                {
                    Checksums.ExistsChecksums(path + pathChecksum);
                    Console.WriteLine("That directory exists already.");
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path + pathChecksum);
                    Console.WriteLine("The directory (checksum) was created successfully at {0}.", Directory.GetCreationTime(path));
                    Checksums.ExistsChecksums(path + pathChecksum);
                }
                // Delete the directory.
                // di.Delete();
                // Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }
    }
}