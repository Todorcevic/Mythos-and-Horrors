using GameRules;
using Sirenix.Utilities;
using Zenject;

namespace GameView
{
    public class ActivatorCardPreseterPresenter : ICardActivator
    {
        [Inject] private readonly CardsManager _cardsManager;

        /*******************************************************************/
        public void ActivateThisCards(params Card[] cards)
        {
            cards.ForEach(card => _cardsManager.Get(card).ActivateToSelect());
        }
    }
}
