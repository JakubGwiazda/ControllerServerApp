﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerServerApp
{
    public class SongData
    {
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public bool IsSongDuration { get; set; }
        public SongData() { }
    }
}
