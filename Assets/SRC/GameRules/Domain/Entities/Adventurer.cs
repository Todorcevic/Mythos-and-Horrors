using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class Adventurer
    {
        public Card AdventurerCard { get; set; }
        public List<Card> Cards { get; set; }
        public int Xp { get; set; }
        public int Injury { get; set; }
        public int Shock { get; set; }
    }
}
