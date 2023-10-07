using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MythsAndHorrors.Tools
{
    public class Pack
    {
        public string name { get; set; }
        public string code { get; set; }
        public int position { get; set; }
        public int cycle_position { get; set; }
        public string available { get; set; }
        public int known { get; set; }
        public int total { get; set; }
        public string url { get; set; }
        public int id { get; set; }
    }
}
