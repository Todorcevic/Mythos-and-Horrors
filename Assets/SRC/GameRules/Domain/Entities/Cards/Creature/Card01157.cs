using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01157 : CardCreature, IStalker, IVictoriable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;

        int IVictoriable.Victory => 10;
        bool IVictoriable.IsVictoryComplete => Health.Value <= 0; //TODO: revisar el reseteo cuando se descarta

        public override IEnumerable<Tag> Tags => new[] { Tag.AncientOne, Tag.Elite };

        /*******************************************************************/
    }
}
