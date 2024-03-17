using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ExhaustReadyCardPresenter : IPresenter<ExhaustCardsGameAction>, IPresenter<ReadyCardGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;

        /*******************************************************************/
        async Task IPresenter<ExhaustCardsGameAction>.PlayAnimationWith(ExhaustCardsGameAction exhaultGamneAction)
        {
            Sequence exhaustSequence = DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(exhaultGamneAction.Cards.Select(card => card.Owner).UniqueOrDefault()));
            foreach (Card card in exhaultGamneAction.Cards)
                exhaustSequence.Join(_cardViewsManager.GetCardView(card).Exhaust());
            await exhaustSequence.AsyncWaitForCompletion();
        }

        async Task IPresenter<ReadyCardGameAction>.PlayAnimationWith(ReadyCardGameAction readyCardGameAction)
        {
            Sequence readySequence = DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(readyCardGameAction.Cards.Select(card => card.Owner).UniqueOrDefault()));
            foreach (Card card in readyCardGameAction.Cards)
                readySequence.Join(_cardViewsManager.GetCardView(card).Ready());
            await readySequence.AsyncWaitForCompletion();
        }

        /*******************************************************************/
        Task IPresenter<ExhaustCardsGameAction>.UndoAnimationWith(ExhaustCardsGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }

        Task IPresenter<ReadyCardGameAction>.UndoAnimationWith(ReadyCardGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
