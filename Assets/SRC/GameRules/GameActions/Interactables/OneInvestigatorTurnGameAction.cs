using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public Effect InvestigateEffect { get; private set; }
        public Effect DrawEffect { get; private set; }
        public Effect TakeResourceEffect { get; private set; }
        public List<Effect> MoveEffects { get; } = new();
        public List<Effect> InvestigatorAttackEffects { get; } = new();
        public List<Effect> InvestigatorConfrontEffects { get; } = new();
        public List<Effect> InvestigatorEludeEffects { get; } = new();
        public List<Effect> PlayFromHandEffects { get; } = new();

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(OneInvestigatorTurnGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(OneInvestigatorTurnGameAction);
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        public OneInvestigatorTurnGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            InteractableGameAction interactableGameAction = new(isUndable: true, Description);
            PreparePassEffect(interactableGameAction);
            PrepareInvestigateEffect(interactableGameAction);
            PrepareMoveEffect(interactableGameAction);
            PrepareInvestigatorAttackEffect(interactableGameAction);
            PrepareInvestigatorConfrontEffect(interactableGameAction);
            PrepareInvestigatorEludeEffect(interactableGameAction);
            PreparePlayFromHandEffect(interactableGameAction);
            PrepareDraw(interactableGameAction);
            PrepareTakeResource(interactableGameAction);
            await _gameActionsProvider.Create(interactableGameAction);
        }

        /*******************************************************************/
        private void PreparePassEffect(InteractableGameAction interactableGameAction)
        {
            interactableGameAction.CreateMainButton()
                .SetLogic(PassTurn);

            async Task PassTurn() =>
                await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, ActiveInvestigator.CurrentTurns.Value));
        }

        /*******************************************************************/
        private void PrepareInvestigateEffect(InteractableGameAction interactableGameAction)
        {
            if (!CanInvestigate()) return;

            InvestigateEffect = interactableGameAction.Create()
                .SetCard(ActiveInvestigator.CurrentPlace)
                .SetInvestigator(ActiveInvestigator)
                .SetLogic(Investigate);

            bool CanInvestigate()
            {
                if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.CurrentPlace?.InvestigationTurnsCost.Value) return false;
                return true;
            }

            async Task Investigate()
            {
                await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, ActiveInvestigator.CurrentPlace.InvestigationTurnsCost.Value));
                await _gameActionsProvider.Create(new InvestigateGameAction(ActiveInvestigator, ActiveInvestigator.CurrentPlace));
            }
        }

        /*******************************************************************/
        private void PrepareMoveEffect(InteractableGameAction interactableGameAction)
        {
            if (ActiveInvestigator.CurrentPlace == null) return;
            foreach (CardPlace cardPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                if (!CanMove()) continue;

                MoveEffects.Add(interactableGameAction.Create()
                    .SetCard(cardPlace)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(Move));

                bool CanMove()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardPlace.MoveTurnsCost.Value) return false;
                    return true;
                }

                async Task Move()
                {
                    await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, cardPlace.MoveTurnsCost.Value));
                    await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(ActiveInvestigator, cardPlace));
                }
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorAttackEffect(InteractableGameAction interactableGameAction)
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorAttack()) continue;

                InvestigatorAttackEffects.Add(interactableGameAction.Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(InvestigatorAttack));

                bool CanInvestigatorAttack()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.InvestigatorAttackTurnsCost.Value) return false;
                    return true;
                }

                async Task InvestigatorAttack()
                {
                    await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, cardCreature.InvestigatorAttackTurnsCost.Value));
                    await _gameActionsProvider.Create(new DecrementStatGameAction(cardCreature.Health, 1));
                }
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorConfrontEffect(InteractableGameAction interactableGameAction)
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorConfront()) continue;

                InvestigatorConfrontEffects.Add(interactableGameAction.Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(InvestigatorConfront));

                bool CanInvestigatorConfront()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.InvestigatorConfronTurnsCost.Value) return false;
                    if (ActiveInvestigator == cardCreature.ConfrontedInvestigator) return false;
                    return true;
                }

                async Task InvestigatorConfront()
                {
                    await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, cardCreature.InvestigatorConfronTurnsCost.Value));
                    await _gameActionsProvider.Create(new MoveCardsGameAction(cardCreature, ActiveInvestigator.DangerZone));
                }
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorEludeEffect(InteractableGameAction interactableGameAction)
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorElude()) continue;

                InvestigatorEludeEffects.Add(interactableGameAction.Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(InvestigatorElude));

                bool CanInvestigatorElude()
                {
                    if (ActiveInvestigator.CurrentTurns.Value < cardCreature.EludeTurnsCost.Value) return false;
                    if (ActiveInvestigator != cardCreature.ConfrontedInvestigator) return false;
                    return true;
                }

                async Task InvestigatorElude()
                {
                    await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, cardCreature.InvestigatorConfronTurnsCost.Value));
                    await _gameActionsProvider.Create(new UpdateStatesGameAction(cardCreature.Exausted, true));
                    await _gameActionsProvider.Create(new MoveCardsGameAction(cardCreature, cardCreature.CurrentPlace.OwnZone));
                }
            }
        }

        /*******************************************************************/
        private void PreparePlayFromHandEffect(InteractableGameAction interactableGameAction)
        {
            foreach (IPlayableFromHand playableFromHand in ActiveInvestigator.HandZone.Cards.OfType<IPlayableFromHand>())
            {
                if (!CanPlayFromHand()) continue;

                PlayFromHandEffects.Add(interactableGameAction.Create()
                    .SetCard(playableFromHand as Card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(PlayFromHand));

                bool CanPlayFromHand()
                {
                    if (ActiveInvestigator.Resources.Value < playableFromHand.ResourceCost.Value) return false;
                    if (ActiveInvestigator.CurrentTurns.Value < playableFromHand.TurnsCost.Value) return false;
                    return true;
                }

                async Task PlayFromHand() =>
                    await _gameActionsProvider.Create(new PlayFromHandGameAction(playableFromHand, ActiveInvestigator));

            }
        }

        /*******************************************************************/
        private void PrepareDraw(InteractableGameAction interactableGameAction)
        {
            if (!CanDraw()) return;

            DrawEffect = interactableGameAction.Create()
                  .SetCard(ActiveInvestigator.CardAidToDraw)
                  .SetInvestigator(ActiveInvestigator)
                  .SetLogic(Draw);
        }

        private bool CanDraw()
        {
            if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.DrawTurnsCost.Value) return false;
            return true;
        }

        private async Task Draw()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, ActiveInvestigator.DrawTurnsCost.Value));
            await _gameActionsProvider.Create(new DrawAidGameAction(ActiveInvestigator));
        }

        /*******************************************************************/
        private void PrepareTakeResource(InteractableGameAction interactableGameAction)
        {
            if (!CanTakeResource()) return;

            TakeResourceEffect = interactableGameAction.Create()
               .SetInvestigator(ActiveInvestigator)
               .SetLogic(TakeResource);
        }

        private bool CanTakeResource()
        {
            if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.TurnsCost.Value) return false;
            return true;
        }

        private async Task TakeResource()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, ActiveInvestigator.TurnsCost.Value));
            await _gameActionsProvider.Create(new GainResourceGameAction(ActiveInvestigator, 1));
        }
    }
}
