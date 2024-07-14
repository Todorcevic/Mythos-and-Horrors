using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CardViewGeneratorComponent : MonoBehaviour
    {
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ZoneViewsManager _zoneViewManager;
        [Inject] private readonly CardsProvider _cardProvider;
        [SerializeField, Required, AssetsOnly] private CardView _cardPrefab;

        /*******************************************************************/
        public void BuildAllCardViews() => _cardProvider.AllCards.ForEach(card => BuildCardView(card));

        public CardView BuildCardView(Card card)
        {
            CardView newCardview = _diContainer.InstantiatePrefabForComponent<CardView>(_cardPrefab, transform);
            newCardview.Init(card, _zoneViewManager.OutZone);
            _cardViewsManager.AddCardView(newCardview);
            return newCardview;
        }
    }
}
