using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ControllerServerApp
{
    public class MusicPlayer : MusicPlayerManager
    {
        static AudioFileReader afr;
        static WaveOutEvent outPutDevice;
        static bool isPlayerRunning = false;
        static Thread player = null;
        static Thread controller = null;

        public MusicPlayer(string basePathToMusic) : base(basePathToMusic)
        {
            outPutDevice = new WaveOutEvent();

        }

        private static void StartPlayingAudioFile(string pathToAudioFile)
        {
            afr = new AudioFileReader(pathToAudioFile);
            outPutDevice.Init(afr);
            outPutDevice.Play();
            while (outPutDevice.PlaybackState != PlaybackState.Stopped)
            {
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
        public void Forward10()
        {
            afr.Skip(10);

        }
        public void Backward10()
        {
            afr.Skip(-10);
        }
        private static void PlayerCommands()
        {
            string command;

            do
            {

                command = Console.ReadLine();
                if (command.Equals("stop"))
                {
                    outPutDevice.Pause();
                }
                if (command.Equals("start"))
                {
                    outPutDevice.Play();

                }
                if (command.Equals("forward"))
                {
                    Console.WriteLine(afr.CurrentTime + " / " + afr.TotalTime);
                    afr.Skip(60);
                    afr.Take(TimeSpan.FromSeconds(10));
                    Console.WriteLine(afr.CurrentTime + " / " + afr.TotalTime);

                }
            } while (!command.Equals("e"));
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
                    controller = new Thread(() => PlayerCommands());
                    controller.Start();
                }
                else
                {
                    afr.Dispose();
                    player.Abort();
                    controller.Abort();
                    
                    OpenSong(ref player,ref controller, pathToChoosenSong);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        private static void OpenSong( ref Thread player, ref Thread controller, string pathToSong)
        {
            player = new Thread(() => StartPlayingAudioFile(pathToSong));
            player.Start();
            controller = new Thread(() => PlayerCommands());
            controller.Start();
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
