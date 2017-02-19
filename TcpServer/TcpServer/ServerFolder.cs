using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace TcpServer
{
    class ServerFolder
    {
        public static void CreateFolder(string path)
        {
            // Specify the directory you want to manipulate.  
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That directory exists already.");
                    //Checksums.ExistsChecksums(path);
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

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            Console.WriteLine("Processed file '{0}'.", path);
        }

        public static string GetFile(string path, string data)
        {
            string cleanData = SplitString(data);

            FileHandler file = JsonConvert.DeserializeObject<FileHandler>(cleanData);

            string fileName = Base64.Base64Decode(file.FileName);
            Console.WriteLine("Check " + fileName);

            string[] fileEntries = Directory.GetFiles(path);
            string response = null;
            List<string> fileNames = new List<string>();

            foreach (var entry in fileEntries)
            {
                string name = Path.GetFileName(entry);
                fileNames.Add(name);
            }

            string filePath = path + @"\" + fileName;

            if (fileNames.Contains(fileName))
            {
                response = FileHandler.ResponseGET200ToJSON(fileName, filePath);
            }
            else
            {
                response = FileHandler.ResponseGET404ToJSON();
            }

            return response;
        }

        public static string PutFile(string path, string data)
        {
            string cleanData = SplitString(data);

            FileHandler file = JsonConvert.DeserializeObject<FileHandler>(cleanData);

            string fileName = Base64.Base64Decode(file.FileName);
            Console.WriteLine("Check " + fileName);
           
            string[] fileEntries = Directory.GetFiles(path);
            string response = null;
            List<string> fileNames = new List<string>();

            foreach (var entry in fileEntries)
            {
                string name = Path.GetFileName(entry);
                fileNames.Add(name);
            }

            if (fileNames.Contains(fileName))
            {
                response = FileHandler.ResponsePUT412ToJson();
            } else
            {
                response = FileHandler.ResponsePUT200ToJson();
            }
            return response;
        }

        public static string DeleteFile(string path, string data)
        {
            string cleanData = SplitString(data);

            FileHandler file = JsonConvert.DeserializeObject<FileHandler>(cleanData);

            string fileName = Base64.Base64Decode(file.FileName);
            string fileChecksum = file.Checksum;
            Console.WriteLine("Check " + fileName);

            string[] fileEntries = Directory.GetFiles(path);
            string response = null;
            List<string> fileNames = new List<string>();

            foreach (var entry in fileEntries)
            {
                string name = Path.GetFileName(entry);
                fileNames.Add(name);
            }
            string filePath = path + @"\" + fileName;

            if (fileNames.Contains(fileName))
            {
                string checksum = Checksums.GetSha1Hash(filePath);
                if (checksum == fileChecksum)
                {
                    File.Delete(filePath);
                    response = FileHandler.ResponseDELETE200ToJSON();
                }
                else
                {
                    response = FileHandler.ResponseDELETE412ToJSON();
                }
            }
            else
            {
                response = FileHandler.ResponseDELETE404ToJSON();
            }

            return response;
        }

        public static string GetList(string path)
        {
            string response = FileHandler.ResponseLIST200ToJSON(path);                  
            return response;
        }

        public static string SplitString(string data)
        {
            string output = data.Substring(data.IndexOf(' ') + 1);
            return output;
        }
    }
}
