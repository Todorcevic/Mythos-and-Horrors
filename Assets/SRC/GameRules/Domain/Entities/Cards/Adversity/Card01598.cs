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
            CreateActivation(2, DiscardActivate, DiscardConditionToActivate, PlayActionType.Activate, new Localization("Activation_Card01598"));
            CreateBuff(CardsToBuff, DecrementStatBuff, RemoveDecrementStatBuff, new Localization("Buff_Card01598"));
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayRevelationFor(Investigator investigator) => await Task.CompletedTask;

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() => IsInPlay.IsTrue ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();

        private async Task DecrementStatBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> allStats = new();
            foreach (CardInvestigator card in cards.OfType<CardInvestigator>())
            {
                allStats[card.Strength] = 1;
                allStats[card.Agility] = 1;
                allStats[card.Power] = 1;
                allStats[card.Intelligence] = 1;
            }

            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(allStats).Execute();
        }
        private async Task RemoveDecrementStatBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> allStats = new();
            foreach (CardInvestigator card in cards.OfType<CardInvestigator>())
            {
                allStats[card.Strength] = 1;
                allStats[card.Agility] = 1;
                allStats[card.Power] = 1;
                allStats[card.Intelligence] = 1;
            }

            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(allStats).Execute();
        }

        /*******************************************************************/
        private async Task DiscardActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        private bool DiscardConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay.IsTrue) return false;
            if ((activeInvestigator.CurrentPlace != ControlOwner.CurrentPlace)) return false;
            return true;
        }
    }
}
