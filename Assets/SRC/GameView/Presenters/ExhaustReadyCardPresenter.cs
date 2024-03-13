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

        async Task IPresenter<ReadyCardGameAction>.PlayAnimationWith(ReadyCardGameAction readyCardGameAction)
        {
            await _cardViewsManager.GetCardView(readyCardGameAction.Card).Ready().AsyncWaitForCompletion();
            //if (readyCardGameAction.Parent is ReadyAllCardsGameAction) _cardViewsManager.GetCardView(readyCardGameAction.Card).Ready();
            //else await _cardViewsManager.GetCardView(readyCardGameAction.Card).Ready().AsyncWaitForCompletion();
        }
    }
}
