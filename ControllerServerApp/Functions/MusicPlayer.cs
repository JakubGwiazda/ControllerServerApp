using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using Newtonsoft.Json;

namespace ControllerServerApp
{
    public class MusicPlayer : MusicPlayerManager
    {
        static AudioFileReader afr;
        static WaveOutEvent outPutDevice;
        static bool isPlayerRunning = false;
        static Thread player = null;
        public static SongData songData;

        public MusicPlayer(string basePathToMusic) : base(basePathToMusic)
        {
            outPutDevice = new WaveOutEvent();
        }

       
        private static void StartPlayingAudioFile(string pathToAudioFile)
        {
            afr = new AudioFileReader(pathToAudioFile);

            songData = new SongData()
            {
                Minutes = afr.TotalTime.Minutes,
                Seconds = afr.TotalTime.Seconds,
                IsSongDuration=true
            };
            MsgSender.SendDataToClient(songData);
            Thread.Sleep(1000);
            outPutDevice.Init(afr);
            outPutDevice.Play();
           // Console.WriteLine(afr.TotalTime.TotalSeconds);
            while (outPutDevice.PlaybackState != PlaybackState.Stopped)
            {

                //Console.WriteLine("\n" + JsonConvert.SerializeObject(songData)); aktualny stan piosenki
                Thread.Sleep(1000); //wymagane do odpowiedniej synchronizacji czasów wyswietlanych u klienta.

                songData.Minutes = afr.CurrentTime.Minutes;
                songData.Seconds = afr.CurrentTime.Seconds;
                songData.IsSongDuration = false;
                if(outPutDevice.PlaybackState!=PlaybackState.Paused)
                MsgSender.SendDataToClient(songData);
            
            }
        }
        public static void StopSong()
        {
            outPutDevice.Pause();

        }
        public static void StartSong()
        {
            outPutDevice.Play();

        }
        public static void Forward10()
        {
            afr.Skip(10);

        }
        public static void Backward10()
        {
            afr.Skip(-10);
        }
        public static void PlayFromSpecificSongPoint(int timeToSkip)
        {
            outPutDevice.Play();
            afr.Skip(timeToSkip);
        }
      
        public static void OpenChoosenSong(string song)
        {
            
            try
            {
                string pathToChoosenSong = FindPathToChoosenAudioFile(song);
                
                if (!isPlayerRunning)
                {
                    isPlayerRunning = true;
                    player = new Thread(() => StartPlayingAudioFile(pathToChoosenSong));
                    player.Start();
                }
                else
                {
                    afr.Dispose();
                    outPutDevice.Stop();
                    player.Abort();
                    player = new Thread(() => StartPlayingAudioFile(pathToChoosenSong));
                    player.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    

        public static string FindPathToChoosenAudioFile(string audioFile)
        {
            List<string> pathsToSongs;
            Func<string, string> GetKey = p => p.Substring(0, 1).ToUpper();
            audioSongs.TryGetValue(GetKey(audioFile), out pathsToSongs);
            return pathsToSongs.FirstOrDefault(x => x.Contains(audioFile));
        }

    }
}
