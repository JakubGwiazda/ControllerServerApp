using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerServerApp
{
    public static class OrderInterpreter
    {
        public static void DoClientOrder(MessageFromClient message)
        {

            string value = message.Message;
            string taskToDo = message.Order;
            switch (taskToDo)
            {
                case "setsound":
                    ServerFunctions.setLevelSound(Convert.ToInt32(Math.Round(Convert.ToDouble(value.Replace('.', ',')))));
                    break;
                case "getsound":
                    MsgSender.SendCurrentSoundLvL();
                    break;
                case "GET_SONG_LIST":
                    MsgSender.SendSongListAsJSON();
                    break;
                case "PLAY_SONG":
                    MusicPlayer.OpenChoosenSong(value);
                    break;
                case "STOP_PLAY":
                    MusicPlayer.StopSong();
                    break;
                case "START_PLAY_AGAIN":
                    MusicPlayer.StartSong();
                    break;
                case "FORWARD":
                    MusicPlayer.Forward10();
                    break;
                case "BACKWARD":
                    MusicPlayer.Backward10();
                    break;
                case "PLAY_FROM_SPECIFIC_POINT":
                    MusicPlayer.PlayFromSpecificSongPoint(Convert.ToInt32(value));
                    break;

            }

        }

    }
}
