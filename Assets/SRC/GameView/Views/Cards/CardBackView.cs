using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CardBackView : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private Sprite _backScenary;
        [SerializeField, Required, AssetsOnly] private Sprite _backAdventurer;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _backFace;

        /*******************************************************************/
        public void SetReverse(Card thisCard)
        {
            _backFace.sprite = thisCard.IsScenaryCard ? _backScenary : _backAdventurer;
        }
    }
}
