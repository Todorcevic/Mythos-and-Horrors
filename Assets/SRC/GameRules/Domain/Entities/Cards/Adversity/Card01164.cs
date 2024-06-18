using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01164 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Terror };
        public Investigator InvestigatorAffected => _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone);

        public State Wasted { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Wasted = CreateState(false);
            CreateForceReaction<PlayInvestigatorGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
            CreateForceReaction<PlayEffectGameAction>(WastedCondition, WasteLogic, GameActionTime.After);
            CreateBuff(CardToBuff, ActivationLogic, DeactivationLogic);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayRevelationFor(Investigator investigator) =>
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Wasted, false));

        /*******************************************************************/
        private async Task WasteLogic(PlayEffectGameAction action) =>
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Wasted, true));

        private bool WastedCondition(PlayEffectGameAction playEffectGameAction)
        {
            if (Wasted.IsActive) return false;
            if ((playEffectGameAction.Effect.PlayActionType & (PlayActionType.Move | PlayActionType.Attack | PlayActionType.Confront)) == 0) return false;
            if (playEffectGameAction.Effect.Investigator != InvestigatorAffected) return false;
            return true;
        }

        /*******************************************************************/
        private async Task ActivationLogic(IEnumerable<Card> cards)
        {
            IEnumerable<Stat> allPlaceStats = cards.OfType<CardPlace>().Select(place => place.MoveTurnsCost);
            IEnumerable<Stat> allCreatureStats = cards.OfType<CardCreature>()
                .SelectMany(creature => new[] { creature.InvestigatorAttackTurnsCost, creature.InvestigatorConfronTurnsCost });
            Dictionary<Stat, int> allStats = allPlaceStats.Concat(allCreatureStats).ToDictionary(stat => stat, stat => 1);

            await _gameActionsProvider.Create(new IncrementStatGameAction(allStats));
        }

        private async Task DeactivationLogic(IEnumerable<Card> cards)
        {
            IEnumerable<Stat> allPlaceStats = cards.OfType<CardPlace>().Select(place => place.MoveTurnsCost);
            IEnumerable<Stat> allCreatureStats = cards.OfType<CardCreature>()
                .SelectMany(creature => new[] { creature.InvestigatorAttackTurnsCost, creature.InvestigatorConfronTurnsCost });
            Dictionary<Stat, int> allStats = allPlaceStats.Concat(allCreatureStats).ToDictionary(stat => stat, stat => 1);

            await _gameActionsProvider.Create(new DecrementStatGameAction(allStats));
        }

        private IEnumerable<Card> CardToBuff()
        {
            return ActivateCondition()
                ? _cardsProvider.GetCardsInPlay().Where(card => card is CardPlace || card is CardCreature)
                : Enumerable.Empty<Card>();

            bool ActivateCondition()
            {
                if (CurrentZone.ZoneType != ZoneType.Danger) return false;
                if (!InvestigatorAffected.IsPlayingTurns.IsActive) return false;
                if (CurrentZone != InvestigatorAffected.DangerZone) return false;
                if (Wasted.IsActive) return false;
                return true;
            }
        }

        /*******************************************************************/
        private bool DiscardCondition(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            if (CurrentZone != playInvestigatorGameAction.ActiveInvestigator.DangerZone) return false;
            return true;
        }

        private async Task DiscardLogic(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            await _gameActionsProvider.Create(new ChallengePhaseGameAction(playInvestigatorGameAction.ActiveInvestigator.Power, 3, $"Challenge: {Info.Name}", this, succesEffect: Discard));
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Wasted, false));

            /*******************************************************************/
            async Task Discard() => await _gameActionsProvider.Create(new DiscardGameAction(this));
        }
    }
}
