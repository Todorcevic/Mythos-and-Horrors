using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class GoalCardView : CardView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _hints;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            _hints.text = Card.Info.Hints.ToString();
        }
    }
}
