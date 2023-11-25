using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AreaAdventurerView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _adventurerZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _handZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _deckZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _discardZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _aidZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _dangerZone;

        public Adventurer Adventurer { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(Adventurer adventurer)
        {
            Adventurer = adventurer;
            _adventurerZone.Init(adventurer.AdventurerZone);
            _handZone.Init(adventurer.HandZone);
            _deckZone.Init(adventurer.DeckZone);
            _discardZone.Init(adventurer.DiscardZone);
            _aidZone.Init(adventurer.AidZone);
            _dangerZone.Init(adventurer.DangerZone);
        }
    }
}
