using UnityEngine;
using Zenject;

namespace GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [Inject] private readonly CardsFactory _cardFactory;
        [Inject] private readonly CardsManager _cardsManager;
        [Inject] private readonly ZonesManager _zonesManager;

        /*******************************************************************/
        private void Start()
        {
            _cardFactory.CreateCard();
            CardView card = _cardsManager.GetCard(0);
            _zonesManager.Investigator.MoveCard(card);
        }
    }
}
