using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class AdventurerRepository
    {
        private List<Adventurer> _adventurers;

        /*******************************************************************/
        public Adventurer GetAdventurer(string code) => _adventurers.First(adventurer => adventurer.AdventurerCard == code);

        public IReadOnlyList<Adventurer> GetAllAdventurers() => _adventurers;

        public void LoadAdventurers(List<Adventurer> adventurers)
        {
            if (_adventurers != null) throw new InvalidOperationException("Adventurers already loaded");
            _adventurers = adventurers ?? throw new ArgumentNullException(nameof(adventurers) + " adventurers cant be null");
        }
    }
}
