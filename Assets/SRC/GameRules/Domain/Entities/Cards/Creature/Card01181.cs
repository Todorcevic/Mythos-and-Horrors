using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01181 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        Investigator ITarget.TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderBy(investigator => investigator.Strength.Value).First();
    }

}
