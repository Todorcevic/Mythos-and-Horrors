//using MythosAndHorrors.GameRules;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Zenject;

//namespace MythosAndHorrors.GameView
//{
//    public class PayAsGroupPresenter : IAsGroupPresenter
//    {
//        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
//        [Inject] private readonly ShowSelectorAsGroup _showSelectorAsGroup;

//        /*******************************************************************/
//        public async Task<Dictionary<Card, int>> SelectWith(GameAction gamAction)
//        {
//            if (gamAction is not PayHintsToGoalGameAction payHintsToGoalGameAction) return default;

//            IEnumerable<Card> cardsToPay = payHintsToGoalGameAction.CardsToPay;




//            IPlayable playableChoose = await _clickHandler.WaitingClick();

//            return null;
//        }
//    }
//}
