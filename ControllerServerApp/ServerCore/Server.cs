using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace ControllerServerApp
{
    class Server
    {
        private static string ipAddress;
        public static TcpClient client;
        private static TcpListener listener;
        public static NetworkStream clientStream;
        public Server() {
            SetServerIPAddress();
        }

        private void SetServerIPAddress()
        {
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach(IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                     ipAddress = address.ToString();
                }
            }
        }
        public static string GetServerIP()
        {
            return ipAddress;
        }

        public void StartServer()
        {
            IPEndPoint ipEP = new IPEndPoint(IPAddress.Parse(ipAddress), 1234);
            listener = new TcpListener(ipEP);
            listener.Start();
            Console.Write($"Serwer zostal wlaczony, jego adres to {ipEP}");
            client = listener.AcceptTcpClient();
            Console.Write("\nPolaczono Klienta, czekam na polecenia");
            while (true) { 
            //Console.Write("\nPolaczono Klienta, czekam na polecenia");
            clientStream = client.GetStream();
            getDataFromClient(clientStream);
            }
        }

        public void getDataFromClient(NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
            string dataToRead = Encoding.UTF8.GetString(bytes);
            OrderInterpreter.DoClientOrder(dataToRead);            
 
        }
    }
}
