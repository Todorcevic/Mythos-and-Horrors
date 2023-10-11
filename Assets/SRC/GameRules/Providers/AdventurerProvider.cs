using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class AdventurerProvider
    {
        private readonly List<Adventurer> _adventurers = new();

        /*******************************************************************/
        public Adventurer GetAdventurer(string code) => _adventurers.First(adventurer => adventurer.AdventurerCardCode.Info.Code == code);

        public IReadOnlyList<Adventurer> GetAllAdventurers() => _adventurers;

        public void AddAdventurer(Adventurer adventurer)
        {
            if (_adventurers.Contains(adventurer)) throw new InvalidOperationException("Adventurer already added");
            _adventurers.Add(adventurer);
        }
    }
}
