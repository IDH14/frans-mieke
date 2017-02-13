using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace Client_IDH14.Models
{
    public class ServerHandler
    {
        public static void Connect(string server, string port)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                //Int32 port = 13000;
                String message = "Hello world";
                int portNumber = Int32.Parse(port);

                TcpClient client = new TcpClient(server, portNumber);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();

        }

        public static void GetFile(string server, string port, string selectedFile) {

            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);

            Byte[] data = System.Text.Encoding.Unicode.GetBytes(FileHandler.FileNameToJSON(selectedFile));

            NetworkStream stream = client.GetStream();
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", data);

            // Buffer to store the response bytes.
            data = new Byte[1024];

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

            Console.WriteLine("Sent: {0}", data);
        }

        public static void GetList(string server, string port)
        {
            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);

            Byte[] data = System.Text.Encoding.Unicode.GetBytes(FileHandler.ListToJSON());

            NetworkStream stream = client.GetStream();
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", data);
        }

        public static void DeleteFile(string server, string port, string selectedFile)
        {
            int portNumber = Int32.Parse(port);
            TcpClient client = new TcpClient(server, portNumber);

            Byte[] data = System.Text.Encoding.Unicode.GetBytes(FileHandler.DeleteToJSON(selectedFile));

            NetworkStream stream = client.GetStream();
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", data);
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
