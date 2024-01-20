using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AreaSceneView : MonoBehaviour
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _dangerDeckZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _dangerDiscardZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _goalZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _plotZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _victoryZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _limboZone;
        [SerializeField, Required, SceneObjectsOnly] private ZoneView _outZone;
        [SerializeField, Required, SceneObjectsOnly] private ZoneView _selector;

        /*******************************************************************/
        public void Init()
        {
            _dangerDeckZone.Init(_chaptersProvider.CurrentScene.DangerDeckZone);
            _dangerDiscardZone.Init(_chaptersProvider.CurrentScene.DangerDiscardZone);
            _goalZone.Init(_chaptersProvider.CurrentScene.GoalZone);
            _plotZone.Init(_chaptersProvider.CurrentScene.PlotZone);
            _victoryZone.Init(_chaptersProvider.CurrentScene.VictoryZone);
            _limboZone.Init(_chaptersProvider.CurrentScene.LimboZone);
            _outZone.Init(_chaptersProvider.CurrentScene.OutZone);
            _selector.Init(_chaptersProvider.CurrentScene.SelectorZone);
        }
    }
}
