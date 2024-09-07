using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ActionTypeController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _fastActionPrefab;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _actionPrefab;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _reactionPrefab;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IPlayableFromHandInTurn playableFromHand)
            {
                if (playableFromHand.IsFast) _fastActionPrefab.gameObject.SetActive(true);
                else _actionPrefab.gameObject.SetActive(true);
            }
            else if (card is CardConditionReaction) _reactionPrefab.gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }
    }
}
