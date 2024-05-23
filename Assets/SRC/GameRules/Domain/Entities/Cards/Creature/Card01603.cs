using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01603 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

        public bool IsUniqueTarget => true;
        public Investigator TargetInvestigator => Owner;

        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Humanoid, Tag.Detective };

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            CreateBuff(CardsToBuff, AddBlankBuff, RemoveBlankBuff);
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff()
        {
            return _investigatorsProvider.GetInvestigatorsInThisPlace(CurrentPlace).Select(investigator => investigator.InvestigatorCard);
        }

        private async Task AddBlankBuff(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(cards.Select(card => card.Blancked), true));
        }

        private async Task RemoveBlankBuff(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(cards.Select(card => card.Blancked), false));
        }
    }
}
