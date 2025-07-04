﻿using System.Collections.Generic;
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
            CreateForceReaction<InvestigatorTurnGameAction>(CheckActionsTypeCondition, CheckActionsTypeLogic, GameActionTime.Before);
        }

        private async Task CheckActionsTypeLogic(InvestigatorTurnGameAction oneInvestigatorTurnGameAction)
        {
            List<CardEffect> cardEffectAffected = oneInvestigatorTurnGameAction.AllPlayableEffects
               .Where(effect => (effect.IsOneTheseActionType(PlayActionType.Move | PlayActionType.Attack | PlayActionType.Elude))).ToList();

            Dictionary<Stat, int> allStats = cardEffectAffected.ToDictionary(cardEffect => cardEffect.ActivateTurnsCost, stat => 1);
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(allStats).Execute();
        }

        private bool CheckActionsTypeCondition(InvestigatorTurnGameAction oneInvestigatorTurnGameAction)
        {
            if (oneInvestigatorTurnGameAction.ActiveInvestigator != InvestigatorAffected) return false;
            return ActivateCondition();
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayRevelationFor(Investigator investigator) =>
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Wasted, false).Execute();

        /*******************************************************************/
        private async Task WasteLogic(PlayEffectGameAction action) =>
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Wasted, true).Execute();

        private bool WastedCondition(PlayEffectGameAction playEffectGameAction)
        {
            if (Wasted.IsActive) return false;
            if (!playEffectGameAction.Effect.IsOneTheseActionType(PlayActionType.Move | PlayActionType.Attack | PlayActionType.Elude)) return false;
            if (playEffectGameAction.Effect.Investigator != InvestigatorAffected) return false;
            return true;
        }

        /*******************************************************************/
        private bool ActivateCondition()
        {
            if (CurrentZone.ZoneType != ZoneType.Danger) return false;
            if (CurrentZone != InvestigatorAffected.DangerZone) return false;
            if (Wasted.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private bool DiscardCondition(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            if (CurrentZone != playInvestigatorGameAction.ActiveInvestigator.DangerZone) return false;
            return true;
        }

        private async Task DiscardLogic(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(playInvestigatorGameAction.ActiveInvestigator.Power, 3,
                new Localization("Challenge_Card01164", CurrentName), this, succesEffect: Discard).Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Wasted, false).Execute();

            /*******************************************************************/
            async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }
    }
}
