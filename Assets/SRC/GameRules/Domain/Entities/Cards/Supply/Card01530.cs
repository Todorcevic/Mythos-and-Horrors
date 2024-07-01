using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01530 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Tool };

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            RemoveStat(PlayFromHandTurnsCost);
            PlayFromHandTurnsCost = CreateStat(0);
            CreateBuff(CardsToBuff, AddIntelligenceBuff, RemoveIntelligenceBuff);
        }
        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() =>
            IsInPlay ?
            new[] { ControlOwner.InvestigatorCard } :
            Enumerable.Empty<Card>();

        private async Task AddIntelligenceBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.OfType<CardInvestigator>().ToDictionary(card => card.Intelligence, card => 1);
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(map).Start();
        }

        private async Task RemoveIntelligenceBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.OfType<CardInvestigator>().ToDictionary(card => card.Intelligence, card => 1);
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(map).Start();
        }
    }
}
