using System;
using System.IO;
using System.Security.Cryptography;

namespace TcpServer
{
    class Checksums
    {
        public static string GetSha1Hash(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                SHA1 sha = new SHA1Managed();
                var data = BitConverter.ToString(sha.ComputeHash(fs));
                return data;
            }
        }
    }
}
