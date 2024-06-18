using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01182 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Omen };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(CreateStat(1), ChallengeToDiscardLogic, ChallengeToDiscardCondition, PlayActionType.Activate);
            CreateBuff(CardsToBuff, ActivateBuff, DeactivateBuff);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayRevelationFor(Investigator investigator) => await Task.CompletedTask;

        /*******************************************************************/
        private async Task DeactivateBuff(IEnumerable<Card> cards)
        {
            CardInvestigator card = cards.OfType<CardInvestigator>().First();

            Dictionary<Stat, int> stats = new()
            {
                { card.Power, 1 }
            };
            if (card.Sanity.Value < card.Info.Sanity) stats.Add(card.Sanity, 1);

            await _gameActionsProvider.Create(new IncrementStatGameAction(stats));
        }

        private async Task ActivateBuff(IEnumerable<Card> cards)
        {
            CardInvestigator card = cards.OfType<CardInvestigator>().First();
            Dictionary<Stat, int> stats = new()
            {
                { card.Power, 1 }
            };
            if (card.Sanity.Value == card.Info.Sanity) stats.Add(card.Sanity, 1);

            await _gameActionsProvider.Create(new DecrementStatGameAction(stats));
        }

        private IEnumerable<Card> CardsToBuff()
        {
            return IsInPlay ? new[] { ControlOwner.InvestigatorCard } : new Card[0];
        }

        /*******************************************************************/
        private async Task ChallengeToDiscardLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new ChallengePhaseGameAction(investigator.Power, 3, $"Discard {Info.Name}", this, succesEffect: Discard));

            async Task Discard()
            {
                await _gameActionsProvider.Create(new DiscardGameAction(this));
            }
        }

        private bool ChallengeToDiscardCondition(Investigator investigator)
        {
            if (CurrentZone.ZoneType != ZoneType.Danger) return false;
            if (investigator.CurrentPlace != ControlOwner.CurrentPlace) return false;
            return true;
        }

    }
}
