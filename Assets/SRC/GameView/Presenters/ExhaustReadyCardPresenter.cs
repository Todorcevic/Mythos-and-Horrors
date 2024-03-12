using System.Threading.Tasks;
using DG.Tweening;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{


    public class ExhaustReadyCardPresenter : IPresenter<ExhaustCardGameAction>, IPresenter<ReadyCardGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        async Task IPresenter<ExhaustCardGameAction>.PlayAnimationWith(ExhaustCardGameAction exhaultGamneAction)
        {
            await _cardViewsManager.GetCardView(exhaultGamneAction.Card).Exhaust().AsyncWaitForCompletion();
        }

        async Task IPresenter<ReadyCardGameAction>.PlayAnimationWith(ReadyCardGameAction gameAction)
        {
            await _cardViewsManager.GetCardView(gameAction.Card).Ready().AsyncWaitForCompletion();

        }
    }
}
