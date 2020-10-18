﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using ControllerServerApp.Functions;
namespace ControllerServerApp
{
    class MusicPlayerManager
    {
        Dictionary<string, List<string>> audioMP3Songs;
        Dictionary<string, List<string>> audioWAVSongs;
        Dictionary<string, List<string>> audioWMASongs;

        static Dictionary<string, List<string>> audioSongs;
        WaveOutEvent outPutDevice;
        AudioFileReader afr;
        public MusicPlayerManager(string basePathToMusic)
        {

            audioMP3Songs = FindPathsToAudioFilesOnHardDrive(basePathToMusic, MusicFormat.mp3.ToString());
            audioWAVSongs = FindPathsToAudioFilesOnHardDrive(basePathToMusic, MusicFormat.wav.ToString());
            audioWMASongs = FindPathsToAudioFilesOnHardDrive(basePathToMusic, MusicFormat.wma.ToString());
            audioSongs =MergeDictionaries(audioMP3Songs, MergeDictionaries(audioWAVSongs, audioWMASongs));
            outPutDevice = new WaveOutEvent();
            
        }
      
       public static Dictionary<string,List<string>> GetSongListWithPaths()
        {
            return audioSongs;
        }
        public static Dictionary<string,List<string>> GetSongList()
        {
            Dictionary<string, List<string>> songList = new Dictionary<string, List<string>>();
           Func<List<string>,List<string>> GetSongTitles = p=>
           {
               List<string> songs = new List<string>();
               foreach(var title in p)
               {
                   songs.Add(title.Remove(0, title.LastIndexOf('\\')+1));
               }
               return songs;
            };

            foreach(var songs in audioSongs)
            {
                songList.Add(songs.Key, GetSongTitles(songs.Value));
            }
            return songList;
        }

        private Dictionary<string,List<string>> MergeDictionaries(Dictionary<string, List<string>> firstDictonary, Dictionary<string, List<string>> secondDictonary)
        {
            Func<List<string>, List<string>,  List<string>> mergeList = (firstList, secondList) =>
            {
                if (firstList == null)
                {
                    return secondList;
                }
                if (secondList == null)
                {
                    return firstList;
                }
                return firstList.Concat(secondList).ToList();
            };
            var leftOuterJoin = (from firstList in firstDictonary
                                 join secondList in secondDictonary on firstList.Key equals secondList.Key into dict
                                 from secondList in dict.DefaultIfEmpty()
                                 select new { position = firstList.Key, audio = mergeList(firstList.Value, secondList.Value) });
          
            var rightOuterJoin = (from secondList in secondDictonary
                                  join firstList in firstDictonary on secondList.Key equals firstList.Key into dict
                                  from firstList in dict.DefaultIfEmpty()
                                  select new { position = secondList.Key, audio = mergeList(firstList.Value, secondList.Value) });
          
            var connectedSets = leftOuterJoin.Union(rightOuterJoin);
            return connectedSets.Where(x => x.position!=null).GroupBy(x => x.position).Select(x => x.FirstOrDefault()).ToDictionary(x=>x.position,x=>x.audio);
        }
      
        private Dictionary<string, List<string>> FindPathsToAudioFilesOnHardDrive(string basePath, string fileExtension)
        {
            string[] pathToFiles = Directory.GetFiles(basePath, "*." + fileExtension, SearchOption.AllDirectories);
            Func<string, string> GetKey = p => Path.GetFileNameWithoutExtension(p).Substring(0, 1).ToUpper();
            var audioFilesDictionary = (from p in pathToFiles group p by GetKey(p) into pair select new { position = pair.Key, audioPath = pair.ToList() }).ToDictionary(x => x.position, x => x.audioPath);
            return audioFilesDictionary;
        }

        public string FindPathToChoosenAudioFile(string audioFile)
        {
            List<string> pathsToSongs;
            Func<string, string> GetKey = p => p.Substring(0, 1).ToUpper();
            audioSongs.TryGetValue(GetKey(audioFile), out pathsToSongs);
            return pathsToSongs.FirstOrDefault(x => x.Contains(audioFile));
        }
        //sekcja z funkcjami odtwarzacza dotyczącymi operacji na plikach
        private  void StartPlayingAudioFile(string pathToAudioFile)
        {
                afr = new AudioFileReader(pathToAudioFile);
                outPutDevice.Init(afr);
                outPutDevice.Play();
                while (outPutDevice.PlaybackState != PlaybackState.Stopped)
                {
                }
          
        }

        private void PlayerCommands()
        {
            string command;

            do
            {
                Console.WriteLine("podaj komende: \n");
                command = Console.ReadLine();
                if (command.Equals("stop"))
                {
                    outPutDevice.Pause();
                }
                if (command.Equals("start"))
                {
                    outPutDevice.Play();
                    
                }
                if(command.Equals("forward"))
                {
                    Console.WriteLine(afr.CurrentTime + " / " + afr.TotalTime);
                    afr.Skip(60);
                    afr.Take(TimeSpan.FromSeconds(10));
                    Console.WriteLine(afr.CurrentTime + " / " + afr.TotalTime);

                }
            } while (!command.Equals("e"));
        }
     
        public void OpenChoosenSong(string song)
        {
            string pathToChoosenSong = FindPathToChoosenAudioFile(song);
            //StartPlayingAudioFile(pathToChoosenSong);
            Thread player = new Thread(()=>StartPlayingAudioFile(pathToChoosenSong));
             player.Start();
           Thread controller = new Thread(()=> PlayerCommands());
            controller.Start();

        }
    }
}



