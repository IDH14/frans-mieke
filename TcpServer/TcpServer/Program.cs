using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                String path = @"c:\\idh14Server";

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();
                ServerFolder.CreateFolder(path);
                // Buffer for reading data
                Byte[] bytes = new Byte[1024];
                String data = null;
                //Checksums.ReadChecksums(path);
                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    string response = null;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.Unicode.GetString(bytes, 0, i);
                        data = cleanMessage(bytes);
                        Console.WriteLine("Received: {0}", data);

                        if (data.StartsWith("GET"))
                        {
                            response = ServerFolder.GetFile(path, data);
                        }
                        else if (data.StartsWith("PUT"))
                        {
                            ServerFolder.PutFile(data);
                        }
                        else if (data.StartsWith("LIST"))
                        {
                            ServerFolder.GetList();
                        }
                        else if (data.StartsWith("DELETE"))
                        {
                            ServerFolder.DeleteFile(data);
                        }

                        // Process the data sent by the client.
                        //data = data.ToUpper();
                        Console.WriteLine("Sent: {0}", response);
                        byte[] msg = System.Text.Encoding.Unicode.GetBytes(response);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", msg);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (IOException ex) {
                Console.WriteLine("Connection closed by client", ex);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
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