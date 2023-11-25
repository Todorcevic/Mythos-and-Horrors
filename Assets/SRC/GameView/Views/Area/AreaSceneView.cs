using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class AreaSceneView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _dangerDeckZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _dangerDiscardZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _goalZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _plotZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _victoryZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _limboZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _outZone;
        [SerializeField] private ZoneView _selector;

        /*******************************************************************/
        public void Init(ZonesProvider zonesProvider)
        {
            _dangerDeckZone.Init(zonesProvider.DangerDeckZone);
            _dangerDiscardZone.Init(zonesProvider.DangerDiscardZone);
            _goalZone.Init(zonesProvider.GoalZone);
            _plotZone.Init(zonesProvider.PlotZone);
            _victoryZone.Init(zonesProvider.VictoryZone);
            _limboZone.Init(zonesProvider.LimboZone);
            _outZone.Init(zonesProvider.OutZone);
            _selector.Init(zonesProvider.SelectorZone);
        }
    }
}
