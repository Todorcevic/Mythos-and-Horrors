using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using Unity.Plastic.Newtonsoft.Json;


namespace MythsAndHorrors.GameRules
{
    public class Adventurer
    {
        public string AdventurerCard { get; set; }
        public List<string> Cards { get; set; }
        public List<string> CardsRequeriment { get; set; }
        public List<(Faction, int)> DeckBuildingCondtions { get; set; }
        public int DeckSize { get; set; }
        public int Xp { get; set; }
        public int Injury { get; set; }
        public int Shock { get; set; }
    }
}
