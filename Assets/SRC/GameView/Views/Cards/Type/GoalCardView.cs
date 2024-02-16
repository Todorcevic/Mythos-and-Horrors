using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class GoalCardView : CardView
    {
        [Title(nameof(GoalCardView))]
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _hints;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            if (Card is CardGoal _goal)
            {
                _hints.SetStat(_goal.Hints);
            }
        }
    }
}
