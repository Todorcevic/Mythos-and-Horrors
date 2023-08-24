using GameRules;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class CardGeneratorComponent : MonoBehaviour, ICardGenerator
    {
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly CardsManager _cardsManager;
        [Inject] private readonly CardRepository _cardRepository;
        [SerializeField, Required, AssetsOnly] private CardView _assetPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _encounterPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _investigatorPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _locationPrefab;

        /*******************************************************************/
        public void BuildCards()
        {
            foreach (Card card in _cardRepository.GetAllCards())
            {
                CardView prefab = GetPrefab(card.Type);
                CardView cardInstantiated = _diContainer.InstantiatePrefabForComponent<CardView>(prefab, transform,
                    new object[] { card.Id, card.Name });
                _cardsManager.Add(cardInstantiated);
            }
        }

        private CardView GetPrefab(CardType cardType) => cardType switch
        {
            CardType.Asset => _assetPrefab,
            CardType.Encounter => _encounterPrefab,
            CardType.Investigator => _investigatorPrefab,
            CardType.Location => _locationPrefab,
            _ => throw new ArgumentNullException("Card type {cardType} not supported"),
        };

    }
}
