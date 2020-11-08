using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
namespace ControllerServerApp
{
    class Program
    {

  
        static void Main(string[] args)
        {
 
           MusicPlayer mp = new MusicPlayer(@"D:\Muzyka");
           Server server = new Server();
                  server.StartServer();
        }
    }
}
