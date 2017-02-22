using Client_IDH14.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Client_IDH14.Models
{
    public class ServerHandler
    {
        public static string GetFile(string server, string port, string selectedFile, string path, string checksumPath)
        {

            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);

            Byte[] data = System.Text.Encoding.Unicode.GetBytes(FileHandler.FileNameToJSON(selectedFile));

            NetworkStream stream = client.GetStream();
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            // Buffer to store the response bytes.
            data = new Byte[1024 * 1024];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.Unicode.GetString(data, 0, bytes);
            responseData = cleanMessage(data);
            System.Diagnostics.Debug.WriteLine("Received: {0}", responseData);

            // Close everything.
            stream.Close();
            client.Close();

            string cleanData = SplitString(responseData);
            FileHandler response = JsonConvert.DeserializeObject<FileHandler>(cleanData);

            if (response.Status == "200")
            {
                //3 stappen: als het bestand lokaal niet bestaat opslaan
                //Als het wel bestaat en checksum komt overeen: melding geven (Bestanden zijn al gelijk)
                //Als het wel bestaat en checksum komt niet overeen: melding geven (Conflict)

                List<string> filenames = FileHandler.GetFilenames();
                var fileName = Base64.Base64Decode(response.FileName);

                if (filenames.Contains(fileName))
                {
                    bool fileInChecksum = Checksums.checksumFileCorrect(path, checksumPath, fileName);
                    if (fileInChecksum == true)
                    {
                        File.Delete(path + fileName);

                        var createFile = File.Create(path + fileName);
                        createFile.Close();

                        File.WriteAllBytes(path + fileName, Convert.FromBase64String(response.Content));
                        Checksums.UpdateChecksums(path + checksumPath, response.Checksum, fileName);
                        return response.Status;
                    }
                }
                else
                {
                    var createFile = File.Create(path + fileName);
                    createFile.Close();

                    File.WriteAllBytes(path + fileName, Convert.FromBase64String(response.Content));
                    Checksums.UpdateNewChecksums(path + checksumPath);
                    return response.Status;
                }

            }
            else if (response.Status == "404")
            {
                return response.Status;
            }
            return "";
        }

        public static string PutFile(string server, string port, string selectedFile)
        {
            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);

            Byte[] data = System.Text.Encoding.Unicode.GetBytes(FileHandler.FileToJSON(selectedFile));

            NetworkStream stream = client.GetStream();
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            // Buffer to store the response bytes.
            data = new Byte[1024 * 1024];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.Unicode.GetString(data, 0, bytes);
            responseData = cleanMessage(data);
            System.Diagnostics.Debug.WriteLine("Received: {0}", responseData);

            // Close everything.
            stream.Close();
            client.Close();

            string cleanData = SplitString(responseData);
            FileHandler response = JsonConvert.DeserializeObject<FileHandler>(cleanData);

            if (response.Status == "200")
            {
                return response.Status;
            }
            else if (response.Status == "404")
            {
                return response.Status;
            }

            return "";
        }

        public static string GetList(string server, string port)
        {
            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);

            Byte[] data = System.Text.Encoding.Unicode.GetBytes(FileHandler.ListToJSON());

            NetworkStream stream = client.GetStream();
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            // Buffer to store the response bytes.
            data = new Byte[1024 * 1024];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.Unicode.GetString(data, 0, bytes);
            responseData = cleanMessage(data);
            System.Diagnostics.Debug.WriteLine("Received: {0}", responseData);

            // Close everything.
            stream.Close();
            client.Close();

            string cleanData = SplitString(responseData);
            FileHandler response = JsonConvert.DeserializeObject<FileHandler>(cleanData);
           
            if (response.Status == "200")
            {
 /*
                foreach (var file in response.Files)
                {
                    Console.WriteLine(file.FileName);
                }*/
                /*
                var jsonObj = new JavaScriptSerializer().Deserialize<RootObj>(json);
                foreach (var obj in jsonObj.objectList)
                {
                    Console.WriteLine(obj.address);
                }*/
                return response.Files;
            }
            return "";
        }

        public static string DeleteFile(string server, string port, string selectedFile, string path, string checksumPath)
        {
            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);
            string checksumFile = Checksums.getChecksumFile(path, checksumPath, selectedFile);
            Byte[] data = System.Text.Encoding.Unicode.GetBytes(FileHandler.DeleteToJSON(selectedFile, checksumFile));

            NetworkStream stream = client.GetStream();
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            // Buffer to store the response bytes.
            data = new Byte[1024 * 1024];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.Unicode.GetString(data, 0, bytes);
            responseData = cleanMessage(data);
            System.Diagnostics.Debug.WriteLine("Received: {0}", responseData);

            // Close everything.
            stream.Close();
            client.Close();

            string cleanData = SplitString(responseData);
            FileHandler response = JsonConvert.DeserializeObject<FileHandler>(cleanData);

            if (response.Status == "200")
            {
                File.Delete(path + selectedFile);
                Checksums.UpdateNewChecksums(path + checksumPath);
                return response.Status;
            }
            else if (response.Status == "404")
            {
                return response.Status;
            }
            else if (response.Status == "412")
            {
                return response.Status;
            }
            return "";
        }

        public static string SplitString(string data)
        {
            string output = data.Substring(data.IndexOf(' ') + 1);
            return output;
        }

        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }
    }
}
