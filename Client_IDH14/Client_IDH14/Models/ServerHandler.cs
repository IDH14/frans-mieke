using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace Client_IDH14.Models
{
    public class ServerHandler
    {
        public static void GetFile(string server, string port, string selectedFile) {

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

                if (filenames.Contains(fileName)) { 
                    
                    if(response.Checksum == response.Checksum)
                    {
                        return;
                    }

                } else
                {
                    // Set c so folder can be checked by client
                    DirectoryInfo c = new DirectoryInfo(@"C:\idh14Client\");

                    var createFile = File.Create(c + fileName);
                    createFile.Close();

                    File.WriteAllBytes(c + fileName, Convert.FromBase64String(response.Content));

                    //To do: message
                }

            } else if (response.Status == "404")
            {

            }

        }

        public static void PutFile(string server, string port, string selectedFile)
        {
            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);

            Byte[] data = System.Text.Encoding.Unicode.GetBytes(FileHandler.FileToJSON(selectedFile));

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream();
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
        }

        public static void GetList(string server, string port)
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

        }

        public static void DeleteFile(string server, string port, string selectedFile, string checksumFile)
        {
            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);

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
