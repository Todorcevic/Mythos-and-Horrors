using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardColosus : CardCreature
    {
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public override bool IsConfronted => MassiveInvestigatorsConfronted.Any();

        public IEnumerable<Investigator> MassiveInvestigatorsConfronted => Exausted.IsActive ? Enumerable.Empty<Investigator>() :
            _investigatorProvider.AllInvestigatorsInPlay.Where(investigator => investigator.CurrentPlace == CurrentPlace);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ConfrontReaction.Disable();
            ConfrontReaction2.Disable();
        }
    }
}
