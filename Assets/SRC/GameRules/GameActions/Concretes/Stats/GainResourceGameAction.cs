using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GainResourceGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public Investigator Investigator { get; }
        public int Amount { get; }

        /*******************************************************************/
        public GainResourceGameAction(Investigator investigator, int amount)
        {
            Investigator = investigator;
            Amount = amount;
            CanBeExecuted = Amount > 0;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new IncrementStatGameAction(Investigator.Resources, Amount));
        }
    }
}
