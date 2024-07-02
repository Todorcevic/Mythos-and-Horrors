using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : InteractableGameAction, IPersonalInteractable
    {
        [Inject] private readonly CardsProvider _cardsProvider;

        public Investigator ActiveInvestigator { get; private set; }
        public CardEffect TakeResourceEffect { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay;

        /*******************************************************************/
        public OneInvestigatorTurnGameAction SetWith()
        {
            SetWith(canBackToThisInteractable: true, mustShowInCenter: false, "Play Turn");
            ActiveInvestigator = PlayInvestigatorGameAction.PlayActiveInvestigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if ((EffectSelected != MainButtonEffect && EffectSelected != UndoEffect)
                || PlayInvestigatorGameAction.PlayActiveInvestigator.HasTurnsAvailable)
                await _gameActionsProvider.Create<OneInvestigatorTurnGameAction>().SetWith().Execute();
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
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(ActiveInvestigator.CurrentTurns, ActiveInvestigator.CurrentTurns.Value).Execute();
        }

        /*******************************************************************/
        private void PreparePlayFromHandEffect()
        {
            foreach (IPlayableFromHand playableFromHand in _cardsProvider.AllCards.OfType<IPlayableFromHand>()
                .Where(playableFromHand => playableFromHand.PlayFromHandCondition.IsTrueWith(ActiveInvestigator)))
            {
                CreateEffect((Card)playableFromHand,
                    playableFromHand.PlayFromHandTurnsCost,
                    PlayFromHand,
                    PlayActionType.PlayFromHand | playableFromHand.PlayFromHandActionType,
                    playedBy: ActiveInvestigator,
                    resourceCost: playableFromHand.ResourceCost);

                async Task PlayFromHand() => await playableFromHand.PlayFromHandCommand.RunWith(this);
            }
        }

        /*******************************************************************/
        private void PrepareInvestigateEffect()
        {
            if (!CanInvestigate()) return;

            CreateEffect(ActiveInvestigator.CurrentPlace,
                ActiveInvestigator.CurrentPlace.InvestigationTurnsCost,
                Investigate,
                PlayActionType.Investigate,
                playedBy: ActiveInvestigator);

            bool CanInvestigate()
            {
                if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.CurrentPlace.InvestigationTurnsCost.Value) return false;
                return true;
            }

            async Task Investigate() => await _gameActionsProvider.Create<InvestigatePlaceGameAction>()
                .SetWith(ActiveInvestigator, ActiveInvestigator.CurrentPlace).Execute();
        }

        /*******************************************************************/
        private void PrepareMoveEffect()
        {
            foreach (CardPlace cardPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                if (!CanMove()) continue;

                CreateEffect(cardPlace,
                    cardPlace.MoveTurnsCost,
                    Move,
                    PlayActionType.Move,
                    ActiveInvestigator);

                bool CanMove()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardPlace.MoveTurnsCost.Value) return false;
                    return true;
                }

                async Task Move() => await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(ActiveInvestigator, cardPlace).Execute();
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorAttackEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorAttack()) continue;

                CreateEffect(cardCreature,
                    cardCreature.InvestigatorAttackTurnsCost,
                    InvestigatorAttack,
                    PlayActionType.Attack,
                    ActiveInvestigator);

                bool CanInvestigatorAttack()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.InvestigatorAttackTurnsCost.Value) return false;
                    return true;
                }

                async Task InvestigatorAttack() =>
                    await _gameActionsProvider.Create<AttackCreatureGameAction>()
                    .SetWith(ActiveInvestigator, cardCreature, amountDamage: 1).Execute();
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorConfrontEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorConfront()) continue;

                CreateEffect(cardCreature,
                    cardCreature.InvestigatorConfronTurnsCost,
                    InvestigatorConfront,
                    PlayActionType.Confront,
                    ActiveInvestigator);

                /*******************************************************************/
                bool CanInvestigatorConfront()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.InvestigatorConfronTurnsCost.Value) return false;
                    if (ActiveInvestigator == cardCreature.ConfrontedInvestigator) return false;
                    return true;
                }

                async Task InvestigatorConfront() =>
                    await _gameActionsProvider.Create<InvestigatorConfrontGameAction>().SetWith(ActiveInvestigator, cardCreature).Execute();
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorEludeEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorElude()) continue;

                CreateEffect(cardCreature,
                    cardCreature.EludeTurnsCost,
                    InvestigatorElude,
                    PlayActionType.Elude,
                    ActiveInvestigator);

                bool CanInvestigatorElude()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.EludeTurnsCost.Value) return false;
                    if (cardCreature.ConfrontedInvestigator != ActiveInvestigator) return false;
                    return true;
                }

                async Task InvestigatorElude() =>
                    await _gameActionsProvider.Create<EludeCreatureGameAction>().SetWith(ActiveInvestigator, cardCreature).Execute();
            }
        }

        /*******************************************************************/
        private void PrepareActivables()
        {
            foreach (Activation<Investigator> activation in _cardsProvider.AllCards.SelectMany(card => card.AllActivations)
                .Where(activation => activation.FullCondition(ActiveInvestigator)))
            {
                if (activation.ActivateTurnsCost.Value > ActiveInvestigator.CurrentTurns.Value) continue;

                CreateEffect(activation.Card,
                    activation.ActivateTurnsCost,
                    Activate,
                    PlayActionType.Activate | activation.PlayAction,
                    ActiveInvestigator);

                async Task Activate() => await activation.PlayFor(ActiveInvestigator);
            }
        }

        /*******************************************************************/
        private void PrepareDraw()
        {
            if (!CanDraw()) return;

            CreateEffect(ActiveInvestigator.CardAidToDraw,
                ActiveInvestigator.DrawTurnsCost,
                Draw,
                PlayActionType.Draw,
                ActiveInvestigator);
        }

        private bool CanDraw()
        {
            if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.DrawTurnsCost.Value) return false;
            return true;
        }

        private async Task Draw() => await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(ActiveInvestigator).Execute();

        /*******************************************************************/
        private void PrepareTakeResource()
        {
            if (!CanTakeResource()) return;
            TakeResourceEffect = CreateEffect(null,
                ActiveInvestigator.BasicActionTurnsCost,
                TakeResource,
                PlayActionType.TakeResource,
                ActiveInvestigator);

            /*******************************************************************/
            async Task TakeResource() => await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(ActiveInvestigator, 1).Execute();
        }

        private bool CanTakeResource()
        {
            if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.BasicActionTurnsCost.Value) return false;
            return true;
        }

    }
}
