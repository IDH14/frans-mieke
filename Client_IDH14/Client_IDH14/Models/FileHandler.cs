using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client_IDH14.Models
{
    public class FileHandler
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Checksum { get; set; }
        public string OriginalChecksum { get; set; }

    }
}