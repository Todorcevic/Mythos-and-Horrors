using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorTurnGameAction : InteractableGameAction, IPersonalInteractable
    {
        [Inject] private readonly CardsProvider _cardsProvider;

        public Investigator ActiveInvestigator { get; private set; }
        public CardEffect TakeResourceEffect { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay.IsTrue;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization)
            => throw new NotImplementedException();

        public InvestigatorTurnGameAction SetWith()
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, new Localization("Interactable_OneInvestigatorTurn", DescriptionParams()));
            ActiveInvestigator = PlayInvestigatorGameAction.PlayActiveInvestigator;
            ExecuteSpecificInitialization();
            return this;

            /*******************************************************************/
            static string[] DescriptionParams()
            {
                CardInvestigator investigatorCard = PlayInvestigatorGameAction.PlayActiveInvestigator.InvestigatorCard;
                return new[] { investigatorCard.Info.Name, investigatorCard.CurrentActions.Value.ToString() };
            }
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if ((EffectSelected != MainButtonEffect && EffectSelected != UndoEffect)
                || PlayInvestigatorGameAction.PlayActiveInvestigator.HasTurnsAvailable.IsTrue)
                await _gameActionsProvider.Create<InvestigatorTurnGameAction>().SetWith().Execute();
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
            CreateMainButton(PassTurn, new Localization("MainButton_OneInvestigatorTurn", ActiveInvestigator.CurrentActions.Value.ToString()));

            async Task PassTurn() =>
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(ActiveInvestigator.CurrentActions, ActiveInvestigator.CurrentActions.Value).Execute();
        }

        private void PreparePlayFromHandEffect()
        {
            foreach (IPlayableFromHandInTurn playableFromHand in _cardsProvider.AllCards.OfType<IPlayableFromHandInTurn>()
                .Where(playableFromHand => playableFromHand.PlayFromHandCondition.IsTrueWith(ActiveInvestigator)))
            {
                CreateCardEffect((Card)playableFromHand,
                    new Stat(playableFromHand.IsFast ? 0 : 1, false),
                    PlayFromHand,
                    PlayActionType.PlayFromHand | playableFromHand.PlayFromHandActionType,
                    playedBy: ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn"),
                    resourceCost: playableFromHand.ResourceCost,
                    cardAffected: playableFromHand.CardAffected?.Invoke());

                /*******************************************************************/
                async Task PlayFromHand() => await playableFromHand.PlayFromHandCommand.RunWith(this);
            }
        }

        private void PrepareInvestigateEffect()
        {
            if (ActiveInvestigator.CurrentPlace.CanBeInvestigated.IsFalse) return;

            CreateCardEffect(ActiveInvestigator.CurrentPlace,
                new Stat(1, false),
                Investigate,
                PlayActionType.Investigate,
                playedBy: ActiveInvestigator,
                new Localization("CardEffect_OneInvestigatorTurn-1"));

            /*******************************************************************/
            async Task Investigate() => await _gameActionsProvider.Create<InvestigatePlaceGameAction>().SetWith(ActiveInvestigator, ActiveInvestigator.CurrentPlace).Execute();
        }

        private void PrepareMoveEffect()
        {
            foreach (CardPlace cardPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                CreateCardEffect(cardPlace,
                    new Stat(1, false),
                    Move,
                    PlayActionType.Move,
                    ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn-2"));

                /*******************************************************************/
                async Task Move() => await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(ActiveInvestigator, cardPlace).Execute();
            }
        }

        private void PrepareInvestigatorAttackEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                CreateCardEffect(cardCreature,
                     new Stat(1, false),
                    InvestigatorAttack,
                    PlayActionType.Attack,
                    ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn-3"));

                /*******************************************************************/
                async Task InvestigatorAttack() => await _gameActionsProvider.Create<AttackCreatureGameAction>().SetWith(ActiveInvestigator, cardCreature, amountDamage: ActiveInvestigator.BasicDamegeToAttack.Value).Execute();
            }
        }

        private void PrepareInvestigatorConfrontEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.ConfrontableCreatures)
            {
                CreateCardEffect(cardCreature,
                    new Stat(1, false),
                    InvestigatorConfront,
                    PlayActionType.Confront,
                    ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn-4"));

                /*******************************************************************/
                async Task InvestigatorConfront() => await _gameActionsProvider.Create<InvestigatorConfrontGameAction>().SetWith(ActiveInvestigator, cardCreature).Execute();
            }
        }

        private void PrepareInvestigatorEludeEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.AllTypeCreaturesConfronted)
            {
                CreateCardEffect(cardCreature,
                    new Stat(1, false),
                    InvestigatorElude,
                    PlayActionType.Elude,
                    ActiveInvestigator,
                    new Localization("CardEffect_OneInvestigatorTurn-5"));

                /*******************************************************************/
                async Task InvestigatorElude() => await _gameActionsProvider.Create<EludeCreatureGameAction>().SetWith(ActiveInvestigator, cardCreature).Execute();
            }
        }

        private void PrepareActivables()
        {
            foreach (Activation<Investigator> activation in _cardsProvider.AllCards.SelectMany(card => card.AllActivations)
                .Where(activation => activation.FullCondition(ActiveInvestigator)))
            {
                CreateCardEffect(activation.Card,
                    activation.ActivateActionsCost,
                    Activate,
                    PlayActionType.Activate | activation.PlayAction,
                    ActiveInvestigator,
                    activation.Localization,
                    cardAffected: activation.CardAffected);

                /*******************************************************************/
                async Task Activate() => await activation.PlayFor(ActiveInvestigator);
            }
        }

        private void PrepareDraw()
        {
            CreateCardEffect(ActiveInvestigator.CardAidToDraw,
                new Stat(1, false),
                Draw,
                PlayActionType.Draw,
                ActiveInvestigator,
                new Localization("CardEffect_OneInvestigatorTurn-6"));

            /*******************************************************************/
            async Task Draw() => await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(ActiveInvestigator).Execute();
        }

        private void PrepareTakeResource()
        {
            TakeResourceEffect = CreateCardEffect(null,
                new Stat(1, false),
                TakeResource,
                PlayActionType.TakeResource,
                ActiveInvestigator,
                new Localization("CardEffect_OneInvestigatorTurn-7"));

            /*******************************************************************/
            async Task TakeResource() => await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(ActiveInvestigator, 1).Execute();
        }
    }
}
