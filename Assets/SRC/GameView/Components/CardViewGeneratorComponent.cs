using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardViewGeneratorComponent : MonoBehaviour
    {
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ImageLoaderUseCase _imageLoaderUseCase;
        [Inject(Id = "OutZone")] private readonly ZoneView _outZoneView;
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
            Task<Sprite> picture = _imageLoaderUseCase.LoadSpriteAsync("Cards/" + card.Info.Code + ".png");
            CardView newCardview = _diContainer.InstantiatePrefabForComponent<CardView>(GetPrefab(card.Info.CardType), transform, new object[] { card, picture });
            newCardview.Off();
            newCardview.SetCurrentZoneView(_outZoneView);
            _cardViewsManager.Add(newCardview);
            return newCardview;
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
