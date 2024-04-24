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

        public Buff AddIntelligence { get; private set; }

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            PlayFromHandTurnsCost = new Stat(0);
            AddIntelligence = _buffsProvider.Create()
                .SetCard(this)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(AddIntelligenceBuff))
                .SetCardsToBuff(CardsToBuff)
                .SetAddBuff(AddIntelligenceBuff)
                .SetRemoveBuff(RemoveIntelligenceBuff);

        }
        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff()
        {
            if (!BuffActivation()) return Enumerable.Empty<Card>();
            return _investigatorsProvider.GetInvestigatorsInThisPlace(Owner.CurrentPlace)
                  .Select(investigator => investigator.InvestigatorCard);

            bool BuffActivation() => _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone)?.AidZone == CurrentZone;
        }

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
