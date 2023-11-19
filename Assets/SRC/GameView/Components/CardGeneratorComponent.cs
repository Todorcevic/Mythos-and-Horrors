using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardGeneratorComponent : MonoBehaviour
    {
        [Inject] private readonly DiContainer _diContainer;
        [SerializeField, Required, AssetsOnly] private CardView _adventurerPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _adventurerDeckPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _adversityPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _creaturePrefab;
        [SerializeField, Required, AssetsOnly] private CardView _placePrefab;
        [SerializeField, Required, AssetsOnly] private CardView _plotPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _goalPrefab;

        /*******************************************************************/
        public CardView BuildCard(Card card)
        {
            CardView cardViewPrefab = GetPrefab(card.Info.CardType);
            return _diContainer.InstantiatePrefabForComponent<CardView>(cardViewPrefab, transform, new object[] { card });
        }

        public List<CardView> BuildCards(IReadOnlyList<Card> allCards)
        {
            List<CardView> cardsView = new();
            foreach (Card card in allCards)
            {
                CardView cardInstantiated = _diContainer.InstantiatePrefabForComponent<CardView>(GetPrefab(card.Info.CardType), transform, new object[] { card });
                cardsView.Add(cardInstantiated);
            }
            return cardsView;
        }

        private CardView GetPrefab(CardType cardType) => cardType switch
        {
            CardType.Adventurer => _adventurerPrefab,
            CardType.Supply or CardType.Talent or CardType.Condition => _adventurerDeckPrefab,
            CardType.Adversity => _adversityPrefab,
            CardType.Creature => _creaturePrefab,
            CardType.Place => _placePrefab,
            CardType.Plot => _plotPrefab,
            CardType.Goal => _goalPrefab,
            _ => throw new ArgumentException($"Card type {cardType} not supported"),
        };
    }
}
