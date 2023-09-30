using GameRules;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class CardGeneratorComponent : MonoBehaviour
    {
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly CardsManager _cardsManager;
        [Inject] private readonly CardRepository _cardRepository;
        [SerializeField, Required, AssetsOnly] private CardView _adventurerPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _adventurerDeckPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _scenarioPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _placePrefab;
        [SerializeField, Required, AssetsOnly] private CardView _plotPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _goalPrefab;

        /*******************************************************************/
        public void BuildCards()
        {
            foreach (Card card in _cardRepository.GetAllCards())
            {
                CardView prefab = GetPrefab(card.Info.CardType);
                CardView cardInstantiated = _diContainer.InstantiatePrefabForComponent<CardView>(prefab, transform,
                    new object[] { card });
                _cardsManager.Add(cardInstantiated);
            }
        }

        private CardView GetPrefab(CardType cardType) => cardType switch
        {
            CardType.Adventurer => _adventurerPrefab,
            CardType.Aid or CardType.Talent or CardType.Condition => _adventurerDeckPrefab,
            CardType.Creature or CardType.Adversity => _scenarioPrefab,
            CardType.Place => _placePrefab,
            CardType.Plot => _plotPrefab,
            CardType.Goal => _goalPrefab,
            _ => throw new ArgumentException($"Card type {cardType} not supported"),
        };
    }
}
