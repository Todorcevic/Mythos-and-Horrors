using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public Effect InvestigateEffect { get; private set; }
        public Effect DrawEffect { get; private set; }
        public Effect TakeResourceEffect { get; private set; }
        public List<Effect> MoveEffects { get; } = new List<Effect>();
        public List<Effect> InvestigatorAttackEffects { get; } = new List<Effect>();
        public List<Effect> InvestigatorConfrontEffects { get; } = new List<Effect>();
        public List<Effect> InvestigatorEludeEffects { get; } = new List<Effect>();
        public List<Effect> PlayFromHandEffects { get; } = new List<Effect>();

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
            PreparePassEffect();
            PrepareInvestigateEffect();
            PrepareMoveEffect();
            PrepareInvestigatorAttackEffect();
            PrepareInvestigatorConfrontEffect();
            PrepareInvestigatorEludeEffect();
            PreparePlayFromHandEffect();
            PrepareDraw();
            PrepareTakeResource();
            await _gameActionProvider.Create(new InteractableGameAction());
        }

        /*******************************************************************/
        private void PreparePassEffect()
        {
            _effectProvider.CreateMainButton()
                .SetCard(null)
                .SetInvestigator(ActiveInvestigator)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PassTurn))
                .SetLogic(PassTurn);

            async Task PassTurn() =>
                await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.Turns.Value));
        }

        /*******************************************************************/
        private void PrepareInvestigateEffect()
        {
            if (!CanInvestigate()) return;

            InvestigateEffect = _effectProvider.Create()
                .SetCard(ActiveInvestigator.CurrentPlace)
                .SetInvestigator(ActiveInvestigator)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Investigate))
                .SetLogic(Investigate);

            bool CanInvestigate()
            {
                if (ActiveInvestigator.Turns.Value < ActiveInvestigator.CurrentPlace?.InvestigationTurnsCost.Value) return false;
                return true;
            }

            async Task Investigate()
            {
                await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.CurrentPlace.InvestigationTurnsCost.Value));
                await _gameActionProvider.Create(new InvestigateGameAction(ActiveInvestigator, ActiveInvestigator.CurrentPlace));
            }
        }

        /*******************************************************************/
        private void PrepareMoveEffect()
        {
            if (ActiveInvestigator.CurrentPlace == null) return;
            foreach (CardPlace cardPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                if (!CanMove()) continue;

                MoveEffects.Add(_effectProvider.Create()
                    .SetCard(cardPlace)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Move))
                    .SetLogic(Move));

                bool CanMove()
                {
                    if (ActiveInvestigator.Turns.Value < cardPlace.MoveTurnsCost.Value) return false;
                    return true;
                }

                async Task Move()
                {
                    await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, cardPlace.MoveTurnsCost.Value));
                    await _gameActionProvider.Create(new MoveInvestigatorToPlaceGameAction(ActiveInvestigator, cardPlace));
                }
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorAttackEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorAttack()) continue;

                InvestigatorAttackEffects.Add(_effectProvider.Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigatorAttack))
                    .SetLogic(InvestigatorAttack));

                bool CanInvestigatorAttack()
                {
                    if (ActiveInvestigator.Turns.Value < cardCreature.InvestigatorAttackTurnsCost.Value) return false;
                    return true;
                }

                async Task InvestigatorAttack()
                {
                    await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, cardCreature.InvestigatorAttackTurnsCost.Value));
                    await _gameActionProvider.Create(new DecrementStatGameAction(cardCreature.Health, 1));
                }
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorConfrontEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorConfront()) continue;

                InvestigatorConfrontEffects.Add(_effectProvider.Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigatorConfront))
                    .SetLogic(InvestigatorConfront));

                bool CanInvestigatorConfront()
                {
                    if (ActiveInvestigator.Turns.Value < cardCreature.InvestigatorConfronTurnsCost.Value) return false;
                    if (ActiveInvestigator == cardCreature.ConfrontedInvestigator) return false;
                    return true;
                }

                async Task InvestigatorConfront()
                {
                    await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, cardCreature.InvestigatorConfronTurnsCost.Value));
                    await _gameActionProvider.Create(new MoveCardsGameAction(cardCreature, ActiveInvestigator.DangerZone));
                }
            }
        }

        /*******************************************************************/
        private void PrepareInvestigatorEludeEffect()
        {
            foreach (CardCreature cardCreature in ActiveInvestigator.CreaturesInSamePlace)
            {
                if (!CanInvestigatorElude()) continue;

                InvestigatorEludeEffects.Add(_effectProvider.Create()
                    .SetCard(cardCreature)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigatorElude))
                    .SetLogic(InvestigatorElude));

                bool CanInvestigatorElude()
                {
                    if (ActiveInvestigator.Turns.Value < cardCreature.EludeTurnsCost.Value) return false;
                    if (ActiveInvestigator != cardCreature.ConfrontedInvestigator) return false;
                    return true;
                }

                async Task InvestigatorElude()
                {
                    await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, cardCreature.InvestigatorConfronTurnsCost.Value));
                    await _gameActionProvider.Create(new ExhaustCardsGameAction(cardCreature));
                    await _gameActionProvider.Create(new MoveCardsGameAction(cardCreature, cardCreature.CurrentPlace.OwnZone));
                }
            }
        }

        /*******************************************************************/
        private void PreparePlayFromHandEffect()
        {
            foreach (IPlayableFromHand playableFromHand in ActiveInvestigator.HandZone.Cards.OfType<IPlayableFromHand>())
            {
                if (!CanPlayFromHand()) continue;

                PlayFromHandEffects.Add(_effectProvider.Create()
                    .SetCard(playableFromHand as Card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PlayFromHand))
                    .SetLogic(PlayFromHand));

                bool CanPlayFromHand()
                {
                    if (ActiveInvestigator.Resources.Value < playableFromHand.ResourceCost.Value) return false;
                    if (ActiveInvestigator.Turns.Value < playableFromHand.TurnsCost.Value) return false;
                    return true;
                }

                async Task PlayFromHand()
                {
                    await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, playableFromHand.TurnsCost.Value));
                    await _gameActionProvider.Create(new PayResourceGameAction(ActiveInvestigator, playableFromHand.ResourceCost.Value));
                    await _gameActionProvider.Create(new DiscardGameAction(playableFromHand as Card));
                }
            }
        }

        /*******************************************************************/
        private void PrepareDraw()
        {
            if (!CanDraw()) return;

            DrawEffect = _effectProvider.Create()
                  .SetCard(ActiveInvestigator.CardAidToDraw)
                  .SetInvestigator(ActiveInvestigator)
                  .SetLogic(Draw)
                  .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Draw));
        }

        private bool CanDraw()
        {
            if (ActiveInvestigator.Turns.Value < ActiveInvestigator.DrawTurnsCost.Value) return false;
            return true;
        }

        private async Task Draw()
        {
            await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.DrawTurnsCost.Value));
            await _gameActionProvider.Create(new DrawAidGameAction(ActiveInvestigator));
        }

        /*******************************************************************/
        private void PrepareTakeResource()
        {
            if (!CanTakeResource()) return;

            TakeResourceEffect = _effectProvider.Create()
               .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(TakeResource))
               .SetInvestigator(ActiveInvestigator)
               .SetLogic(TakeResource);
        }

        private bool CanTakeResource()
        {
            if (ActiveInvestigator.Turns.Value < ActiveInvestigator.ResourceTurnsCost.Value) return false;
            return true;
        }

        private async Task TakeResource()
        {
            await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.ResourceTurnsCost.Value));
            await _gameActionProvider.Create(new GainResourceGameAction(ActiveInvestigator, 1));
        }
    }
}
