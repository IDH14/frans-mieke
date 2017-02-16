using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TcpServer
{
    class Checksums
    {
        static String file = @"\checksums.csv";
        public static void ExistsChecksums(String path)
        {
            if (!File.Exists(path + file))
            {
                CreateChecksums(path);
            }
            else
            {
                Console.WriteLine("That file exists already.");
            }
        }
        public static void CreateChecksums(String path)
        {
            var createFile = File.Create(path + file);
            createFile.Close();
            Console.WriteLine("The file (Checksums.csv) was created successfully at {0}.", Directory.GetCreationTime(path));
        }

        public static void ReadChecksums(String path)
        {
            String line;
            try

            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(path + file);

                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the lie to console window
                    Console.WriteLine(line);
                    //Read the next line
                    line = sr.ReadLine();
                }

                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)

            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally

            {
                Console.WriteLine("Executing finally block.");
            }
        }
    }
}
