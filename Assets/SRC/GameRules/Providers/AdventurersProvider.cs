using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class AdventurersProvider
    {
        private readonly List<Adventurer> _adventurers = new();

        public IReadOnlyList<Adventurer> AllAdventurers => _adventurers;
        public Adventurer Leader => _adventurers.First();

        /*******************************************************************/
        public void AddAdventurer(Adventurer adventurer)
        {
            if (_adventurers.Contains(adventurer)) throw new InvalidOperationException("Adventurer already added");
            _adventurers.Add(adventurer);
        }

        public int GetAdventurerPosition(Adventurer adventurer) => _adventurers.IndexOf(adventurer) + 1;
    }
}
