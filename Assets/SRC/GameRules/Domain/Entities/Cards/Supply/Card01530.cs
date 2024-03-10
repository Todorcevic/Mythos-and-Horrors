using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01530 : CardSupply
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

        public Buff AddIntelligence { get; private set; }

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            AddIntelligence = _buffsProvider.Create(this, "BuffExample", CardsToBuff, AddIntelligenceBuff, RemoveIntelligenceBuff);
        }

        /*******************************************************************/
        private List<Card> CardsToBuff()
        {
            if (!BuffActivation()) return new List<Card>();
            return _investigatorsProvider.GetInvestigatorsInThisPlace(Owner.CurrentPlace)
                  .Select(investigator => investigator.InvestigatorCard).Cast<Card>().ToList();

            bool BuffActivation() => _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone)?.AidZone == CurrentZone;
        }

        private async Task AddIntelligenceBuff(Card card)
        {
            if (card is not CardInvestigator cardInvestigator) return;
            await _gameActionProvider.Create(new IncrementStatGameAction(cardInvestigator.Intelligence, 1));
        }

        private async Task RemoveIntelligenceBuff(Card card)
        {
            if (card is not CardInvestigator cardInvestigator) return;
            await _gameActionProvider.Create(new DecrementStatGameAction(cardInvestigator.Intelligence, 1));
        }
    }
}
