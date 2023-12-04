using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AreaSceneView : MonoBehaviour
    {
        [Inject] private readonly ZonesProvider _zonesProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _dangerDeckZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _dangerDiscardZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _goalZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _plotZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _victoryZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _limboZone;
        [SerializeField, Required, SceneObjectsOnly] private ZoneView _outZone;
        [SerializeField] private ZoneView _selector;

        /*******************************************************************/
        public void Init()
        {
            _dangerDeckZone.Init(_zonesProvider.DangerDeckZone);
            _dangerDiscardZone.Init(_zonesProvider.DangerDiscardZone);
            _goalZone.Init(_zonesProvider.GoalZone);
            _plotZone.Init(_zonesProvider.PlotZone);
            _victoryZone.Init(_zonesProvider.VictoryZone);
            _limboZone.Init(_zonesProvider.LimboZone);
            _outZone.Init(_zonesProvider.OutZone);
            _selector.Init(_zonesProvider.SelectorZone);
        }
    }
}
