using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerServerApp
{
    public static class OrderInterpreter
    {
        private static string order;
        private static string value;
        private static void GetOrderFromClientMsg(string message)
        {
            order = message.Substring(0, message.IndexOf(':'));
            value = message.Substring(message.IndexOf(':') + 1).Replace('.',',');

        }
        private static void GetRequestFromClientMsg(string message)
        {
            order = message.Substring(0, message.IndexOf('\0'));
        }
        public static void DoClientOrder(string message)
        {
            if (message.Contains(":"))
                GetOrderFromClientMsg(message);
            else
                GetRequestFromClientMsg(message);

            string taskToDo = order;
            switch (taskToDo)
            {
                case "setsound":
                    
                    ServerFunctions.setLevelSound(Convert.ToInt32(Math.Round(Convert.ToDouble(value))));
                    break;
                case "getsound":
                    MsgSender.SendCurrentSoundLvL();
                    break;
                case "GET_SONG_LIST":
                    MsgSender.SendSongListAsJSON();
                    break;
            }

        }

    }
}
