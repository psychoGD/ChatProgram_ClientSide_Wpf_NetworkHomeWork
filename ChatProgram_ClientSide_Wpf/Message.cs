﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram_ClientSide_Wpf
{
    public class Message
    {
        public User User { get; set; }
        public string message { get; set; }
        public bool FromClient { get; set; }
        public DateTime dateTime { get; set; }
    }
}
