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
            // MusicPlayerManager a = new MusicPlayerManager(@"D:\Muzyka");
           //  Console.WriteLine(a.FindPathToChoosenAudioFile("Czas miłości"));
            // a.OpenChoosenSong("Czas miłości");

           // MsgSender.SendSongList();
            //a.FindPathsToAudioFilesOnHardDrive(@"D:\Muzyka","mp3");
            // a.StartPlayingAudioFile(@"D:\Muzyka\Studio Accantus - Złota Kolekcja\Studio Accantus - Złota Kolekcja\Jeszcze dzień - Nędznicy.mp3");
            MusicPlayerManager mpm = new MusicPlayerManager(@"D:\Muzyka");
           


       //  var a =   MusicPlayerManager.GetSongList();
    //        var json = JsonConvert.SerializeObject(MusicPlayerManager.GetSongList());

                Server server = new Server();
                  server.StartServer();
        }
    }
}
