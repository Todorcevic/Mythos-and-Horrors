using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public Effect TakeResourceEffect { get; private set; }
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
            CreateMainButton(PassTurn, "Finish");

            async Task PassTurn() =>
                await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, ActiveInvestigator.CurrentTurns.Value));
        }

        /*******************************************************************/
        private void PreparePlayFromHandEffect()
        {
            foreach (IPlayableFromHand playableFromHand in _cardsProvider.AllCards.OfType<IPlayableFromHand>()
                .Where(playableFromHand => playableFromHand.PlayFromHandCondition.IsTrueWith(this)))
            {
                CreateEffect((Card)playableFromHand, PlayFromHand, PlayActionType.PlayFromHand | playableFromHand.PlayFromHandActionType, playedBy: ActiveInvestigator);

                async Task PlayFromHand() =>
                    await _gameActionsProvider.Create(new PlayFromHandGameAction(playableFromHand, ActiveInvestigator));
            }
        }

        /*******************************************************************/
        private void PrepareInvestigateEffect()
        {
            if (!CanInvestigate()) return;

            CreateEffect(ActiveInvestigator.CurrentPlace, Investigate, PlayActionType.Investigate, playedBy: ActiveInvestigator);

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

                CreateEffect(cardPlace, Move, PlayActionType.Move, ActiveInvestigator);

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

                CreateEffect(cardCreature, InvestigatorAttack, PlayActionType.Attack, ActiveInvestigator);

                bool CanInvestigatorAttack()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.InvestigatorAttackTurnsCost.Value) return false;
                    return true;
                }

                async Task InvestigatorAttack() => await _gameActionsProvider.Create(new PlayAttackGameAction(ActiveInvestigator, cardCreature));
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorConfrontEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorConfront()) continue;

                CreateEffect(cardCreature, InvestigatorConfront, PlayActionType.Confront, ActiveInvestigator);

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

                CreateEffect(cardCreature, InvestigatorElude, PlayActionType.Elude, ActiveInvestigator);

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
        private void PrepareActivables()
        {
            foreach (Card activable in _cardsProvider.AllCards.Where(card => card.IsActivable))
            {
                foreach (Activation activation in activable.AllActivations)
                {
                    if (activation.FullCondition(ActiveInvestigator))
                        CreateEffect(activable, Activate, PlayActionType.Activate | activation.PlayActionType, ActiveInvestigator);

                    async Task Activate() => await _gameActionsProvider.Create(new PlayActivateCardGameAction(activation, ActiveInvestigator));
                }
            }
        }

        /*******************************************************************/
        private void PrepareDraw()
        {
            if (!CanDraw()) return;

            CreateEffect(ActiveInvestigator.CardAidToDraw, Draw, PlayActionType.Draw, ActiveInvestigator);
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
            TakeResourceEffect = CreateEffect(null, TakeResource, PlayActionType.TakeResource, ActiveInvestigator);

            /*******************************************************************/
            async Task TakeResource() => await _gameActionsProvider.Create(new PlayTakeResourceGameAction(ActiveInvestigator));
        }

        private bool CanTakeResource()
        {
            if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.BasicActionTurnsCost.Value) return false;
            return true;
        }

    }
}
