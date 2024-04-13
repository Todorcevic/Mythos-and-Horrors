using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01116 : CardCreature, ITarget, IStalker, IGhoul
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        Investigator ITarget.Investigator => _investigatorsProvider.GetTopInvestigatorsStrength;
    }
}
