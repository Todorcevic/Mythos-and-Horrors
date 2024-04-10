using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01505 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        private Card AmuletoDeWendy => _cardsProvider.GetCard("01514"); //TODO: Poner nombre real de la carta

        /*******************************************************************/
        public override async Task StarEffect()
        {
            if (!AmuletoDeWendy.IsInPlay) return;
            _gameActionsProvider.CurrentChallenge.IsAutoSucceed = true;
            await Task.CompletedTask;
        }

        public override int StarValue() => 0;
    }
}
