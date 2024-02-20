using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayInvestigatorTurn : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;

        public Investigator Investigator { get; }
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        public PlayInvestigatorTurn(Investigator investigator)
        {
            Investigator = investigator;
        }


        /*******************************************************************/
        protected override Task ExecuteThisPhaseLogic()
        {
            throw new System.NotImplementedException();
        }
    }
}
