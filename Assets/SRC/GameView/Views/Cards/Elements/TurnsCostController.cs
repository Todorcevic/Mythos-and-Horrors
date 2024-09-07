using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class TurnsCostController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _fastTurnPrefab;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _turnPrefab;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _reactionPrefab;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IPlayableFromHandInTurn playableFromHand)
            {
                if (playableFromHand.IsFast) _fastTurnPrefab.gameObject.SetActive(true);
                else _turnPrefab.gameObject.SetActive(true);
            }
            else if (card is CardConditionReaction) _reactionPrefab.gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }
    }
}
