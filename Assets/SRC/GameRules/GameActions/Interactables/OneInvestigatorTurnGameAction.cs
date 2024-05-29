using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public Effect InvestigateEffect { get; private set; }
        public Effect DrawEffect { get; private set; }
        public Effect TakeResourceEffect { get; private set; }
        public List<Effect> MoveEffects { get; } = new();
        public List<Effect> InvestigatorAttackEffects { get; } = new();
        public List<Effect> InvestigatorConfrontEffects { get; } = new();
        public List<Effect> InvestigatorEludeEffects { get; } = new();
        public List<Effect> PlayFromHandEffects { get; } = new();
        public List<Effect> PlayActivableEffects { get; } = new();

        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay;

        /*******************************************************************/
        public OneInvestigatorTurnGameAction() :
            base(canBackToThisInteractable: true, mustShowInCenter: false, "Play Turn", PlayInvestigatorGameAction.PlayActiveInvestigator)
        { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if ((EffectSelected != MainButtonEffect && EffectSelected != UndoEffect)
                || PlayInvestigatorGameAction.PlayActiveInvestigator.HasTurnsAvailable)
                await _gameActionsProvider.Create(new OneInvestigatorTurnGameAction());
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            PreparePassEffect();
            PrepareInvestigateEffect();
            PrepareMoveEffect();
            PrepareInvestigatorAttackEffect();
            PrepareInvestigatorConfrontEffect();
            PrepareInvestigatorEludeEffect();
            PreparePlayFromHandEffect();
            PrepareActivables();
            PrepareDraw();
            PrepareTakeResource();
        }

        private void PreparePassEffect()
        {
            CreateMainButton().SetLogic(PassTurn);

            async Task PassTurn() =>
                await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, ActiveInvestigator.CurrentTurns.Value));
        }

        /*******************************************************************/
        private void PrepareInvestigateEffect()
        {
            if (!CanInvestigate()) return;

            InvestigateEffect = Create()
                .SetCard(ActiveInvestigator.CurrentPlace)
                .SetInvestigator(ActiveInvestigator)
                .SetLogic(Investigate);

            bool CanInvestigate()
            {
                if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.CurrentPlace?.InvestigationTurnsCost.Value) return false;
                return true;
            }

            async Task Investigate() =>
                await _gameActionsProvider.Create(new PlayInvestigateGameAction(ActiveInvestigator, ActiveInvestigator.CurrentPlace));
        }

        /*******************************************************************/
        private void PrepareMoveEffect()
        {
            if (ActiveInvestigator.CurrentPlace == null) return;
            foreach (CardPlace cardPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                if (!CanMove()) continue;

                MoveEffects.Add(Create()
                    .SetCard(cardPlace)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(Move));

                bool CanMove()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardPlace.MoveTurnsCost.Value) return false;
                    return true;
                }

                async Task Move() => await _gameActionsProvider.Create(new PlayMoveInvestigatorGameAction(ActiveInvestigator, cardPlace));
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorAttackEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorAttack()) continue;

                InvestigatorAttackEffects.Add(Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(InvestigatorAttack));

                bool CanInvestigatorAttack()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.InvestigatorAttackTurnsCost.Value) return false;
                    return true;
                }

                async Task InvestigatorAttack() => await _gameActionsProvider.Create(new PlayBasicAttackGameAction(ActiveInvestigator, cardCreature));
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorConfrontEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorConfront()) continue;

                InvestigatorConfrontEffects.Add(Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(InvestigatorConfront));

                bool CanInvestigatorConfront()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.InvestigatorConfronTurnsCost.Value) return false;
                    if (ActiveInvestigator == cardCreature.ConfrontedInvestigator) return false;
                    return true;
                }

                async Task InvestigatorConfront() =>
                    await _gameActionsProvider.Create(new PlayConfronGameAction(ActiveInvestigator, cardCreature));
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorEludeEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorElude()) continue;

                InvestigatorEludeEffects.Add(Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(InvestigatorElude));

                bool CanInvestigatorElude()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.EludeTurnsCost.Value) return false;
                    if (cardCreature.ConfrontedInvestigator != ActiveInvestigator) return false;
                    return true;
                }

                async Task InvestigatorElude() =>
                    await _gameActionsProvider.Create(new PlayEludeGameAction(ActiveInvestigator, cardCreature));
            }
        }

        /*******************************************************************/
        private void PreparePlayFromHandEffect()
        {
            foreach (IPlayableFromHand playableFromHand in _cardsProvider.AllCards.OfType<IPlayableFromHand>()
                .Where(playableFromHand => DefaultCondition(playableFromHand)))
            {
                PlayFromHandEffects.Add(Create()
                    .SetCard(playableFromHand as Card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(PlayFromHand));

                async Task PlayFromHand() =>
                    await _gameActionsProvider.Create(new PlayFromHandGameAction(playableFromHand, ActiveInvestigator));
            }

            bool DefaultCondition(IPlayableFromHand playableFromHand)
            {
                if (playableFromHand is not Card card) return false;
                if (card.CurrentZone != ActiveInvestigator.HandZone) return false;
                if (playableFromHand.ResourceCost.Value > ActiveInvestigator.Resources.Value) return false;
                if (playableFromHand.PlayFromHandTurnsCost.Value > ActiveInvestigator.CurrentTurns.Value) return false;
                if (!playableFromHand.SpecificConditionToPlayFromHand()) return false;
                return true;
            }
        }

        /*******************************************************************/
        private void PrepareActivables()
        {
            foreach (Card activable in _cardsProvider.AllCards.Where(card => card.IsActivable))
            {
                foreach (Activation activation in activable.AllActivations)
                {
                    if (activation.FullCondition(ActiveInvestigator))
                        PlayActivableEffects.Add(Create()
                           .SetCard(activable)
                           .SetInvestigator(ActiveInvestigator)
                           .SetLogic(Activate));

                    async Task Activate() => await _gameActionsProvider.Create(new PlayActivateCardGameAction(activation, ActiveInvestigator));
                }
            }
        }

        /*******************************************************************/
        private void PrepareDraw()
        {
            if (!CanDraw()) return;

            DrawEffect = Create()
                  .SetCard(ActiveInvestigator.CardAidToDraw)
                  .SetInvestigator(ActiveInvestigator)
                  .SetLogic(Draw);
        }

        private bool CanDraw()
        {
            if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.DrawTurnsCost.Value) return false;
            return true;
        }

        private async Task Draw() => await _gameActionsProvider.Create(new PlayDrawCardGameAction(ActiveInvestigator));

        /*******************************************************************/
        private void PrepareTakeResource()
        {
            if (!CanTakeResource()) return;

            TakeResourceEffect = Create()
               .SetInvestigator(ActiveInvestigator)
               .SetLogic(TakeResource);
        }

        private bool CanTakeResource()
        {
            if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.TurnsCost.Value) return false;
            return true;
        }

        private async Task TakeResource() => await _gameActionsProvider.Create(new PlayTakeResourceGameAction(ActiveInvestigator));
    }
}
