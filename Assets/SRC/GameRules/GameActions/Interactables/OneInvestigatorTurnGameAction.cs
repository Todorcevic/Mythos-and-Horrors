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
            InteractableGameAction interactableGameAction = new();
            PreparePassEffect(interactableGameAction);
            PrepareUndoEffect(interactableGameAction);
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
                .SetCard(null)
                .SetInvestigator(ActiveInvestigator)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PassTurn))
                .SetLogic(PassTurn);

            async Task PassTurn() =>
                await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, ActiveInvestigator.CurrentTurns.Value));
        }

        private void PrepareUndoEffect(InteractableGameAction interactableGameAction)
        {
            interactableGameAction.CreateUndoButton()
                .SetInvestigator(ActiveInvestigator)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(UndoEffect))
                .SetLogic(UndoEffect);

            async Task UndoEffect()
            {
                InteractableGameAction lastPlayInvestigator = await _gameActionsProvider.UndoLast();
                _gameActionsProvider.GetLastActive<PlayInvestigatorGameAction>()?.SetInvestigator(null); //Esto es correcto NO TOCAR
                await _gameActionsProvider.Create(new PlayInvestigatorGameAction(_gameActionsProvider.GetActiveInvestigator()));
            }
        }

        /*******************************************************************/
        private void PrepareInvestigateEffect(InteractableGameAction interactableGameAction)
        {
            if (!CanInvestigate()) return;

            InvestigateEffect = interactableGameAction.Create()
                .SetCard(ActiveInvestigator.CurrentPlace)
                .SetInvestigator(ActiveInvestigator)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Investigate))
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
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Move))
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
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigatorAttack))
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
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigatorConfront))
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
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigatorElude))
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
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PlayFromHand))
                    .SetLogic(PlayFromHand));

                bool CanPlayFromHand()
                {
                    if (ActiveInvestigator.Resources.Value < playableFromHand.ResourceCost.Value) return false;
                    if (ActiveInvestigator.CurrentTurns.Value < playableFromHand.TurnsCost.Value) return false;
                    return true;
                }

                async Task PlayFromHand()
                {
                    await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, playableFromHand.TurnsCost.Value));
                    await _gameActionsProvider.Create(new PayResourceGameAction(ActiveInvestigator, playableFromHand.ResourceCost.Value));
                    await _gameActionsProvider.Create(new DiscardGameAction(playableFromHand as Card));
                }
            }
        }

        /*******************************************************************/
        private void PrepareDraw(InteractableGameAction interactableGameAction)
        {
            if (!CanDraw()) return;

            DrawEffect = interactableGameAction.Create()
                  .SetCard(ActiveInvestigator.CardAidToDraw)
                  .SetInvestigator(ActiveInvestigator)
                  .SetLogic(Draw)
                  .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Draw));
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
               .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(TakeResource))
               .SetInvestigator(ActiveInvestigator)
               .SetLogic(TakeResource);
        }

        private bool CanTakeResource()
        {
            if (ActiveInvestigator.CurrentTurns.Value < ActiveInvestigator.ResourceTurnsCost.Value) return false;
            return true;
        }

        private async Task TakeResource()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(ActiveInvestigator.CurrentTurns, ActiveInvestigator.ResourceTurnsCost.Value));
            await _gameActionsProvider.Create(new GainResourceGameAction(ActiveInvestigator, 1));
        }
    }
}
