using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;

namespace ControllerServerApp
{
    public static class MsgSender
    {

        public  async static void SendCurrentSoundLvL()
        {
            try
            {
                string soundLvL = ServerFunctions.GetLevelSound();
                byte[] bytesToSend = System.Text.Encoding.UTF8.GetBytes(soundLvL);
                await Server.clientStream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error", "" + ex.ToString(), "OK");
            }
        }
        public async static void SendSongListAsJSON()
        {
            try
            {
                var json = JsonConvert.SerializeObject(MusicPlayerManager.GetSongList());
                byte[] bytesToSend = System.Text.Encoding.UTF8.GetBytes(json);
                Console.WriteLine("/nWyslano liste");
                await Server.clientStream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error", "" + ex.ToString(), "OK");
            }
        }
        public async static void SendSongList()
        {
            try
            {
                Dictionary<string, List<string>> songs = MusicPlayerManager.GetSongListWithPaths();
                var json = JsonConvert.SerializeObject(MusicPlayerManager.GetSongListWithPaths());
                var binFormatter = new BinaryFormatter();
                var mStream = new MemoryStream();
                binFormatter.Serialize(mStream, songs);
                byte[] bytesToSend=mStream.ToArray();
                await Server.clientStream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
            }catch(Exception ex)
            {
                Console.WriteLine("Error", "" + ex.ToString(), "OK");
            }
        }
    

    }

}
