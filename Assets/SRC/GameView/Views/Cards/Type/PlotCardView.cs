using MythsAndHorrors.EditMode;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.PlayMode
{
    public class PlotCardView : CardView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _eldritch;

        /*******************************************************************/
        protected override void SetAll()
        {
            _eldritch.text = Card.Info.Eldritch.ToString();
        }
    }
}
