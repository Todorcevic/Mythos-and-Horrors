using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class GoalCardView : CardView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _hints;

        /*******************************************************************/
        protected override void SetAll()
        {
            _hints.text = Card.Info.Hints.ToString();
        }
    }
}
