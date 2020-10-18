using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Diagnostics;

namespace ControllerServerApp
{

    public class HostDevices
    {
        private static CoreAudioDevice _defaultPlaybackDevice;
        public static CoreAudioDevice DefaultPlaybackDevice { get
            {
                if (_defaultPlaybackDevice == null)
                {
                    _defaultPlaybackDevice= new CoreAudioController().DefaultPlaybackDevice;
                }
                return _defaultPlaybackDevice;
            } }
    }
    class ServerFunctions
    {
      
        public static void setLevelSound(int level)
        {
            HostDevices.DefaultPlaybackDevice.Volume = level;
        }

        public static string GetLevelSound()
        {
         return Convert.ToString(HostDevices.DefaultPlaybackDevice.Volume);
        }

        
    }
}
