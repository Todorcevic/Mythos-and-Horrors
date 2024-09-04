using System.Diagnostics.CodeAnalysis;
using System;
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
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay.IsTrue;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization)
            => throw new NotImplementedException();

        public OneInvestigatorTurnGameAction SetWith()
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, new Localization("Interactable_OneInvestigatorTurn", DescriptionParams()));
            ActiveInvestigator = PlayInvestigatorGameAction.PlayActiveInvestigator;
            ExecuteSpecificInitialization();
            return this;

            /*******************************************************************/
            static string[] DescriptionParams()
            {
                CardInvestigator investigatorCard = PlayInvestigatorGameAction.PlayActiveInvestigator.InvestigatorCard;
                return new[] { investigatorCard.Info.Name, investigatorCard.CurrentTurns.Value.ToString() };
            }
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if ((EffectSelected != MainButtonEffect && EffectSelected != UndoEffect)
                || PlayInvestigatorGameAction.PlayActiveInvestigator.HasTurnsAvailable.IsTrue)
                await _gameActionsProvider.Create<OneInvestigatorTurnGameAction>().SetWith().Execute();
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            if (!CanBeExecuted) return;
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
            CreateMainButton(PassTurn, new Localization("MainButton_OneInvestigatorTurn", ActiveInvestigator.CurrentTurns.Value.ToString()));

            async Task PassTurn() =>
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(ActiveInvestigator.CurrentTurns, ActiveInvestigator.CurrentTurns.Value).Execute();
        }

        /*******************************************************************/
        private void PreparePlayFromHandEffect()
        {
            foreach (IPlayableFromHandInTurn playableFromHand in _cardsProvider.AllCards.OfType<IPlayableFromHandInTurn>()
                .Where(playableFromHand => playableFromHand.PlayFromHandCondition.IsTrueWith(ActiveInvestigator)))
            {
                CreateCardEffect((Card)playableFromHand,
                    playableFromHand.PlayFromHandTurnsCost,
                    PlayFromHand,
                    PlayActionType.PlayFromHand | playableFromHand.PlayFromHandActionType,
                    playedBy: ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn"),  //Check if can propagate the localizableCode from IPlayableFromHand
                    resourceCost: playableFromHand.ResourceCost,
                    cardAffected: playableFromHand.CardAffected?.Invoke());

                async Task PlayFromHand() => await playableFromHand.PlayFromHandCommand.RunWith(this);
            }
        }

        /*******************************************************************/
        private void PrepareInvestigateEffect()
        {
            if (!CanInvestigate()) return;

            CreateCardEffect(ActiveInvestigator.CurrentPlace,
                ActiveInvestigator.CurrentPlace.InvestigationTurnsCost,
                Investigate,
                PlayActionType.Investigate,
                playedBy: ActiveInvestigator,
                new Localization("CardEffect_OneInvestigatorTurn-1"));

            bool CanInvestigate()
            {
                if (!ActiveInvestigator.CurrentPlace.CanBeInvestigated.IsTrue) return false;
                if (!ActiveInvestigator.CanInvestigate.IsTrue) return false;
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

                CreateCardEffect(cardPlace,
                    cardPlace.MoveTurnsCost,
                    Move,
                    PlayActionType.Move,
                    ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn-2"));

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

                CreateCardEffect(cardCreature,
                    cardCreature.InvestigatorAttackTurnsCost,
                    InvestigatorAttack,
                    PlayActionType.Attack,
                    ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn-3"));

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

                CreateCardEffect(cardCreature,
                    cardCreature.InvestigatorConfronTurnsCost,
                    InvestigatorConfront,
                    PlayActionType.Confront,
                    ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn-4"));

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

                CreateCardEffect(cardCreature,
                    cardCreature.EludeTurnsCost,
                    InvestigatorElude,
                    PlayActionType.Elude,
                    ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn-5"));

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

                CreateCardEffect(activation.Card,
                    activation.ActivateTurnsCost,
                    Activate,
                    PlayActionType.Activate | activation.PlayAction,
                    ActiveInvestigator,
                    activation.Localization,
                    cardAffected: activation.CardAffected);

                async Task Activate() => await activation.PlayFor(ActiveInvestigator);
            }
        }

        /*******************************************************************/
        private void PrepareDraw()
        {
            if (!CanDraw()) return;

            CreateCardEffect(ActiveInvestigator.CardAidToDraw,
                ActiveInvestigator.DrawTurnsCost,
                Draw,
                PlayActionType.Draw,
                ActiveInvestigator,
                new Localization("CardEffect_OneInvestigatorTurn-6"));
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

            TakeResourceEffect = CreateCardEffect(null,
                ActiveInvestigator.BasicActionTurnsCost,
                TakeResource,
                PlayActionType.TakeResource,
                ActiveInvestigator,
                new Localization("CardEffect_OneInvestigatorTurn-7"));

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
