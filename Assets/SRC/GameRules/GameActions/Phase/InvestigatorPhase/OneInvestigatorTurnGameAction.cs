using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : PhaseGameAction //2.2.1	Investigator takes an action, if able.
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        public OneInvestigatorTurnGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            CheckIfCanInvestigate();
            CheckIfCanMove();
            await _gameActionFactory.Create(new InteractableGameAction(isMandatary: false));
        }

        private void CheckIfCanInvestigate()
        {
            if (!ActiveInvestigator.CanInvestigate) return;
            ActiveInvestigator.CurrentPlace.AddEffect(ActiveInvestigator, _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigateEffect), InvestigateEffect);

            Task InvestigateEffect() => _gameActionFactory.Create(new InvestigateGameAction(ActiveInvestigator, ActiveInvestigator.CurrentPlace));
        }

        private void CheckIfCanMove()
        {
            foreach (CardPlace connectedPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                if (connectedPlace.CanMoveWithThis(ActiveInvestigator))
                    connectedPlace.AddEffect(ActiveInvestigator, _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(MoveEffect), MoveEffect);

                Task MoveEffect() => _gameActionFactory.Create(new MoveToPlaceGameAction(ActiveInvestigator, connectedPlace));
            }
        }
    }
}
