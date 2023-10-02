using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorCardPreseterPresenter : ICardActivator
    {
        [Inject] private readonly CardsViewManager _cardsManager;

        /*******************************************************************/
        public void ActivateThisCards(params Card[] cards)
        {
            cards.ForEach(card => _cardsManager.Get(card).ActivateToSelect());
        }
    }
}
