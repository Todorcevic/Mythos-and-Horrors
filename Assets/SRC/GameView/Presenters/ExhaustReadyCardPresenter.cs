using System.Threading.Tasks;
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
            _cardViewsManager.GetCardView(exhaultGamneAction.Card).Exhaust();
            await Task.CompletedTask;
        }

        Task IPresenter<ReadyCardGameAction>.PlayAnimationWith(ReadyCardGameAction gameAction)
        {
            _cardViewsManager.GetCardView(gameAction.Card).Ready();
            return Task.CompletedTask;
        }
    }
}
