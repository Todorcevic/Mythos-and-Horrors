using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01598 : CardAdversity
    {
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Curse };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(CreateStat(2), DiscardActivate, DiscardConditionToActivate);
            CreateBuff(CardsToBuff, DecrementStatBuff, RemoveDecrementStatBuff);
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();

        private async Task DecrementStatBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> allStats = new();
            foreach (var card in cards.OfType<CardInvestigator>())
            {
                allStats[card.Strength] = 1;
                allStats[card.Agility] = 1;
                allStats[card.Power] = 1;
                allStats[card.Intelligence] = 1;
            }

            await _gameActionsProvider.Create(new DecrementStatGameAction(allStats));
        }
        private async Task RemoveDecrementStatBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> allStats = new();
            foreach (var card in cards.OfType<CardInvestigator>())
            {
                allStats[card.Strength] = 1;
                allStats[card.Agility] = 1;
                allStats[card.Power] = 1;
                allStats[card.Intelligence] = 1;
            }

            await _gameActionsProvider.Create(new IncrementStatGameAction(allStats));
        }

        /*******************************************************************/
        private async Task DiscardActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        private bool DiscardConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if ((activeInvestigator.CurrentPlace != ControlOwner.CurrentPlace)) return false;
            return true;
        }
    }
}
