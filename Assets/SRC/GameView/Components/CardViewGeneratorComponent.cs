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
        [Inject] private readonly ZoneViewsManager _zoneViewManager;
        [SerializeField, Required, AssetsOnly] private CardView _investigatorPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _investigatorDeckPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _adversityPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _creaturePrefab;
        [SerializeField, Required, AssetsOnly] private CardView _placePrefab;
        [SerializeField, Required, AssetsOnly] private CardView _plotPrefab;
        [SerializeField, Required, AssetsOnly] private CardView _goalPrefab;
        [SerializeField, Required, AssetsOnly] private AvatarCardView _avatarCardPrefab;

        /*******************************************************************/
        public CardView Clone(CardView cardView, Transform parent) =>
            _diContainer.InstantiatePrefabForComponent<CardView>(cardView, parent, new object[] { cardView.Card });

        public CardView BuildCard(Card card)
        {
            CardView newCardview = _diContainer.InstantiatePrefabForComponent<CardView>(GetPrefab(card), transform, new object[] { card });
            newCardview.Off();
            newCardview.SetCurrentZoneView(_zoneViewManager.OutZone);
            _cardViewsManager.AddCardView(newCardview);
            return newCardview;
        }

        private CardView GetPrefab(Card card) => card switch
        {
            CardInvestigator => _investigatorPrefab,
            CardSupply or CardTalent or CardCondition => _investigatorDeckPrefab,
            CardAdversity => _adversityPrefab,
            CardCreature => _creaturePrefab,
            CardPlace => _placePrefab,
            CardPlot => _plotPrefab,
            CardGoal => _goalPrefab,
            CardAvatar => _avatarCardPrefab,
            _ => throw new ArgumentException($"Card type {card} not supported"),
        };
    }
}
