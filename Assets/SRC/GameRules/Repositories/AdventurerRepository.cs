using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class AdventurerRepository
    {
        private List<Adventurer> _adventurers;

        /*******************************************************************/
        public void SetAdventurers(List<Adventurer> adventurers)
        {
            if (_adventurers != null) throw new InvalidOperationException("Adventurers already loaded");
            _adventurers = adventurers ?? throw new ArgumentNullException(nameof(adventurers) + " adventurers cant be null");
        }

        public Adventurer GetAdventurer(string code) => _adventurers.First(adventurer => adventurer.AdventurerCardCode.Info.Code == code);

        public IReadOnlyList<Adventurer> GetAllAdventurers() => _adventurers;
    }
}
