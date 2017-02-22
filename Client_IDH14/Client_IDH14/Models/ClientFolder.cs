using System.IO;

namespace Client_IDH14.Models
{
    public class ClientFolder
    {
        public static void CreateFolder(string path)
        {
            // Specify the directory you want to manipulate.  
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
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
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path + pathChecksum);
                    Checksums.ExistsChecksums(path + pathChecksum);
                }
            }
            finally { }
        }
    }
}