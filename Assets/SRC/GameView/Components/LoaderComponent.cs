using DG.Tweening;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [Inject] private readonly CardsFactory _cardFactory;
        [Inject] private readonly ZonesManager _zonesManager;

        /*******************************************************************/
        private async void Start()
        {
            for (int i = 0; i < 30; i++)
            {
                CardView card = _cardFactory.CreateCard();

                await _zonesManager.FrontCamera.MoveCard(card).AsyncWaitForCompletion();
                _zonesManager.AssetsDeck.MoveCard(card);
            }

            for (int i = 0; i < 30; i++)
            {
                CardView card = _cardFactory.CreateCard();

                await _zonesManager.AssetsDiscard.MoveCard(card).AsyncWaitForCompletion();
            }
        }
    }
}
