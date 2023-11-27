using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardActivatorPresenter : ICardActivator
    {
        [Inject] private readonly CardViewsManager _cardsManager;

        /*******************************************************************/
        public void ActivateThisCards(params Card[] cards)
        {
            cards.ForEach(card => _cardsManager.Get(card).ActivateToSelect());
        }
    }
}
