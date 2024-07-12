using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class CostController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _cost;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IPlayableFromHand platableFromHand)
            {
                SetCostWith(platableFromHand.ResourceCost);
            }
            else if (card is CardGoal cardGoal)
            {
                SetCostWith(cardGoal.Hints);
            }
        }

        private void SetCostWith(Stat stat)
        {
            _cost.SetStat(stat);
            _cost.gameObject.SetActive(true);
        }
    }
}
