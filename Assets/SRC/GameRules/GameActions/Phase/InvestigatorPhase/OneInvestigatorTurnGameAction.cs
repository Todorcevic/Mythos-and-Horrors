using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : GameAction //2.2.1	Investigator takes an action, if able.
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator ActiveInvestigator { get; }

        /*******************************************************************/
        public OneInvestigatorTurnGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {



            await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.InvestigatorCard.Turns, 1));

        }
    }
}
