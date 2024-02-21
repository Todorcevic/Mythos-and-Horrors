using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : PhaseGameAction //2.2	Next investigator's turn begins.
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsCanStart)
            {
                investigator.AvatarCard.AddEffect(investigator, _textsProvider.GameText.DEFAULT_VOID_TEXT, ChooseInvestigatorEffect);

                Task ChooseInvestigatorEffect() => _gameActionFactory.Create(new PlayInvestigatorGameAction(investigator));
            }

            await _gameActionFactory.Create(new InteractableGameAction(isMandatary: true));
        }
    }
}
