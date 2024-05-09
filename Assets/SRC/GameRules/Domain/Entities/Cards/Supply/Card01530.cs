using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01530 : CardSupply
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            PlayFromHandTurnsCost = CreateStat(0);
            _buffsProvider.Create()
               .SetCard(this)
               .SetDescription(nameof(AddIntelligenceBuff))
               .SetCardsToBuff(CardsToBuff)
               .SetAddBuff(AddIntelligenceBuff)
               .SetRemoveBuff(RemoveIntelligenceBuff);

        }
        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() =>
            BuffActivation() ?
            new List<Card> { _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone).InvestigatorCard } :
            Enumerable.Empty<Card>();

        private bool BuffActivation() => _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone)?.AidZone == CurrentZone;

        private async Task AddIntelligenceBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.ToDictionary(card => ((CardInvestigator)card).Intelligence, card => 1);
            await _gameActionsProvider.Create(new IncrementStatGameAction(map));
        }

        private async Task RemoveIntelligenceBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.ToDictionary(card => ((CardInvestigator)card).Intelligence, card => 1);
            await _gameActionsProvider.Create(new DecrementStatGameAction(map));
        }
    }
}
