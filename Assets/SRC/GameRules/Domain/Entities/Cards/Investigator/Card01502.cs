using System.Collections.Generic;
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
            await new SafeForeach<ITome>(DrawAid, GetTomes).Execute();

            async Task DrawAid(ITome tome) => await _gameActionsProvider.Create(new DrawAidGameAction(Owner));
            IEnumerable<ITome> GetTomes() => Owner.CardsInPlay.OfType<ITome>();
        }
    }
}
