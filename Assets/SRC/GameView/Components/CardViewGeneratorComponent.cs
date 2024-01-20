using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardViewGeneratorComponent : MonoBehaviour
    {
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [SerializeField, Required, AssetsOnly] private CardView _investigatorPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _investigatorDeckPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _adversityPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _creaturePrefab;
        [SerializeField, Required, AssetsOnly] private CardView _placePrefab;
        [SerializeField, Required, AssetsOnly] private CardView _plotPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _goalPrefab;

        /*******************************************************************/
        public CardView Create(Card card)
        {
            CardView newCardview = _diContainer.InstantiatePrefabForComponent<CardView>(GetPrefab(card.Info.CardType), transform, new object[] { card });
            newCardview.Off();
            newCardview.SetCurrentZoneView(_zoneViewsManager.OutZone);
            return newCardview;
        }

        public CardView BuildCard(Card card)
        {
            CardView newCardview = Create(card);
            _cardViewsManager.Add(newCardview);
            return newCardview;
        }

        private CardView GetPrefab(CardType cardType) => cardType switch
        {
            CardType.Investigator => _investigatorPrefab,
            CardType.Supply or CardType.Talent or CardType.Condition => _investigatorDeckPrefab,
            CardType.Adversity => _adversityPrefab,
            CardType.Creature => _creaturePrefab,
            CardType.Place => _placePrefab,
            CardType.Plot => _plotPrefab,
            CardType.Goal => _goalPrefab,
            _ => throw new ArgumentException($"Card type {cardType} not supported"),
        };
    }
}
