using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BpDmTerminal.Models
{
    public class CardResponse
    {
        public bool success { get; set; }
        public string uid { get; set; }
        public string error { get; set; }
        public long? elapsedMilliseconds { get; set; }
        public bool isCardEnd { get; set; }
        public int attempts { get; set; }
    }
}