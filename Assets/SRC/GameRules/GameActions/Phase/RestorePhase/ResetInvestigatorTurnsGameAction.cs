using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResetInvestigatorTurnsGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public ResetInvestigatorTurnsGameAction(Investigator gameActionType)
        {
            Investigator = gameActionType;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionProvider.Create(new UpdateStatGameAction(Investigator.Turns, Investigator.Turns.MaxValue));
        }
    }
}
