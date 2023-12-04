using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

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

        public bool IsFree => Adventurer == null;
        public Adventurer Adventurer { get; private set; }

        /*******************************************************************/
        public void Init(Adventurer adventurer)
        {
            name = "AdventurerZones" + adventurer.AdventurerCard.Info.Code;
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
