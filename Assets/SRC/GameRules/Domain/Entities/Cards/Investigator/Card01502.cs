using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01502 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public override async Task StarEffect()
        {
            _gameActionsProvider.CurrentChallenge.SuccesEffects.Add(DrawCards);
            await Task.CompletedTask;
        }

        public override int StarValue() => 0;

        private async Task DrawCards()
        {
            int amount = Owner.CardsInPlay.OfType<ITome>().Count();
            for (int i = 0; i < amount; i++)
            {
                await _gameActionsProvider.Create(new DrawAidGameAction(Owner));
            }
        }
    }
}
