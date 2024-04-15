using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01503 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public override async Task StarEffect()
        {
            _gameActionsProvider.CurrentChallenge.SuccesEffects.Add(DrawResources);
            await Task.CompletedTask;
        }

        public override int StarValue() => 2;

        private async Task DrawResources()
        {
            await _gameActionsProvider.Create(new GainResourceGameAction(Owner, 2));
        }
    }
}
