using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

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
              
               NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach(NetworkInterface ni in networkInterfaces)
            {
               
                if(ni.NetworkInterfaceType!=NetworkInterfaceType.Loopback && ni.NetworkInterfaceType!=NetworkInterfaceType.Tunnel
                    && ni.OperationalStatus==OperationalStatus.Up && !ni.Name.StartsWith("vEthernet") && !ni.Description.Contains("Hyper-v") 
                    && !ni.Description.Contains("VirtualBox"))
                {
                    IPInterfaceProperties properties = ni.GetIPProperties();
                 
                    foreach (IPAddressInformation address in properties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;
                        ipAddress = address.Address.ToString();
                    }
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
            try { 
            byte[] bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
            string dataToRead = Encoding.UTF8.GetString(bytes);
            OrderInterpreter.DoClientOrder(EncodeClientMessage(dataToRead));
            }catch(Exception ex)
            {
                Console.WriteLine("\n" + "Blad polaczenia z klientem. Poleczenie zamkniete. Nalezy ponownie sie polaczyc." + "\n");
                listener.Stop();
                StartServer();
            }
            }

        public MessageFromClient EncodeClientMessage(string messageFromClient)
        {
        
            var a = JsonConvert.DeserializeObject<MessageFromClient>(messageFromClient);
            return a;
        
        }
        
    }

    public class MessageFromClient
    {
        public string Order { get; set; }
        public string Message { get; set; }
    }
}
