using MythsAndHorrors.EditMode;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.PlayMode
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
