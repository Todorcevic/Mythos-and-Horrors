using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class CardsFactory : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private CardView _assetPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _encounterPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _investigatorPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _locationPrefab;

        [Inject] private readonly CardsManager _cardsManager;

        /*******************************************************************/
        public void CreateCard()
        {
            CardView cardInstantiated = Instantiate(_assetPrefab, transform);

            _cardsManager.AddCard(cardInstantiated);
        }

    }
}
