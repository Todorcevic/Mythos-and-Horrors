using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : PhaseGameAction //2.2.1	Investigator takes an action, if able.
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        public Stat DrawCost { get; private set; }
        public Condition CanDraw { get; private set; }
        public Condition CanInvestigate { get; private set; }

        /*******************************************************************/
        public OneInvestigatorTurnGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            DrawCost = new Stat(1);
            CanDraw = new Condition(() => ActiveInvestigator.Turns.Value >= DrawCost.Value);
            CanInvestigate = new Condition(() => ActiveInvestigator.Turns.Value >= ActiveInvestigator.CurrentPlace.InvestigationCost.Value);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            CheckIfCanMove();
            CheckIfCanInvestigate();
            CheckIfCanDraw();
            await _gameActionFactory.Create(new InteractableGameAction(isMandatary: false));
        }

        private void CheckIfCanMove()
        {
            foreach (CardPlace connectedPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                _effectProvider.Add(new(
                    connectedPlace,
                    ActiveInvestigator,
                    _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(MoveEffect),
                    new Condition(() => connectedPlace.CanMoveWithThis(ActiveInvestigator)),
                    MoveEffect));

                async Task MoveEffect()
                {
                    await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, connectedPlace.MoveCost.Value));
                    await _gameActionFactory.Create(new MoveToPlaceGameAction(ActiveInvestigator, connectedPlace));
                }
            }
        }

        private void CheckIfCanInvestigate()
        {
            _effectProvider.Add(new(
                ActiveInvestigator.CurrentPlace,
                ActiveInvestigator,
                _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigateEffect),
                CanInvestigate,
                InvestigateEffect));

            async Task InvestigateEffect()
            {
                await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.CurrentPlace.InvestigationCost.Value));
                await _gameActionFactory.Create(new InvestigateGameAction(ActiveInvestigator, ActiveInvestigator.CurrentPlace));
            }
        }

        private void CheckIfCanDraw()
        {
            _effectProvider.Add(new(
            ActiveInvestigator.CardToDraw,
            ActiveInvestigator,
            _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(DrawEffect),
            CanDraw,
            DrawEffect));

            async Task DrawEffect()
            {
                await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, DrawCost.Value));
                await _gameActionFactory.Create(new DrawGameAction(ActiveInvestigator));
            }
        }
    }
}
