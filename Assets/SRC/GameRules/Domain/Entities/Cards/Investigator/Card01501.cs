using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01501 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

        public Buff AddResourceCost { get; private set; }

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            AddResourceCost = _buffsProvider.Create()
                .SetCard(this)
                .SetDescription(_textsProvider.GameText.CARD01501_BUFF)
                .SetCardsToBuff(CardsToBuff)
                .SetAddBuff(AddReductionCostBuff)
                .SetRemoveBuff(RemoveReductionCostBuff);
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff()
        {
            if (CurrentZone != Owner.InvestigatorZone) return new List<Card>();
            return Owner.HandZone.Cards.Where(card => card is CardCondition || card is CardSupply);
        }

        private async Task AddReductionCostBuff(Card card)
        {
            if (card is CardCondition cardTalent)
                await _gameActionProvider.Create(new DecrementStatGameAction(cardTalent.ResourceCost, 1));

            if (card is CardSupply cardSupply)
                await _gameActionProvider.Create(new DecrementStatGameAction(cardSupply.ResourceCost, 1));
        }

        private async Task RemoveReductionCostBuff(Card card)
        {
            if (card is CardCondition cardTalent)
                await _gameActionProvider.Create(new IncrementStatGameAction(cardTalent.ResourceCost, 1));

            if (card is CardSupply cardSupply)
                await _gameActionProvider.Create(new IncrementStatGameAction(cardSupply.ResourceCost, 1));
        }
    }
}
