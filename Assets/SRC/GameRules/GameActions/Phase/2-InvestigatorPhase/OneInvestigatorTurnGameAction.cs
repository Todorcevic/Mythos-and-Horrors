using System.Collections.Generic;
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
        public List<Effect> MoveEffects { get; } = new List<Effect>();

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
            await _gameActionProvider.Create(new InteractableGameAction());
        }

        /*******************************************************************/
        private void PreparePassEffect()
        {
            _effectProvider.CreateMainButton()
                .SetCard(null)
                .SetInvestigator(ActiveInvestigator)
                .SetCanPlay(CanPassTurn)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PassTurn))
                .SetLogic(PassTurn);

            bool CanPassTurn() => true;

            async Task PassTurn() =>
                await _gameActionProvider.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.Turns.Value));
        }

        /*******************************************************************/
        private void PrepareInvestigateEffect()
        {
            InvestigateEffect = _effectProvider.Create()
                .SetCard(ActiveInvestigator.CurrentPlace)
                .SetInvestigator(ActiveInvestigator)
                .SetCanPlay(CanInvestigate)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Investigate))
                .SetLogic(Investigate);

            bool CanInvestigate()
            {
                if (ActiveInvestigator.Turns.Value < ActiveInvestigator.CurrentPlace.InvestigationTurnsCost.Value) return false;
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
            foreach (CardPlace cardPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                MoveEffects.Add(_effectProvider.Create()
                    .SetCard(cardPlace)
                    .SetInvestigator(ActiveInvestigator)
                    .SetCanPlay(CanMove)
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
    }
}
