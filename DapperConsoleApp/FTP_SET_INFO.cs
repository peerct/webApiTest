using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperConsoleApp
{
   public class FTP_SET_INFO
    {
        public int ID { get; set; }
        public string FTPIP { get; set; }
        public int FTPPORT { get; set; }
        public string FTPUSER { get; set; }
        public string FTPPWD { get; set; }
        public string SERVER_ROOT_PATH { get; set; }
    }
}
