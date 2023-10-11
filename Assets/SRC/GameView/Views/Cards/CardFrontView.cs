using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CardFrontView : MonoBehaviour
    {
        [SerializeField, AssetsOnly] private Sprite _intrepidTemplate;
        [SerializeField, AssetsOnly] private Sprite _versatileTemplate;
        [SerializeField, AssetsOnly] private Sprite _valiantTemplate;
        [SerializeField, AssetsOnly] private Sprite _esotericTemplate;
        [SerializeField, AssetsOnly] private Sprite _scholarlyTemplate;
        [SerializeField, AssetsOnly] private Sprite _neutralTemplate;

        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;

        /*******************************************************************/

        public void SetPicture(Card thisCard)
        {
            _picture.sprite = GetPicture(thisCard);
            _template.sprite = SetTemplate(thisCard);
        }

        private Sprite GetPicture(Card thisCard)
        {
            throw new NotImplementedException();
        }

        private Sprite SetTemplate(Card thisCard)
        {
            return thisCard.Info.Faction switch
            {
                Faction.Intrepid => _intrepidTemplate,
                Faction.Versatile => _versatileTemplate,
                Faction.Valiant => _valiantTemplate,
                Faction.Esoteric => _esotericTemplate,
                Faction.Scholarly => _scholarlyTemplate,
                Faction.Neutral => _neutralTemplate,
                _ => throw new Exception("Faction not found"),
            };
        }
    }
}
