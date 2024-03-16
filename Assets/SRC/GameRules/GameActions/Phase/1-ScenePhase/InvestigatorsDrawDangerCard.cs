using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsDrawDangerCard : PhaseGameAction
    {
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(InvestigatorsDrawDangerCard);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(InvestigatorsDrawDangerCard);
        public override Phase MainPhase => Phase.Scene;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay)
            {
                await _gameActionProvider.Create(new DrawDangerGameAction(investigator));
            }
        }
    }
}
