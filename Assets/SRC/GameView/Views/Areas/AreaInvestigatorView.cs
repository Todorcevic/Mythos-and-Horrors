using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{

    public class AreaInvestigatorView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _investigatorZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _handZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _deckZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _discardZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _aidZone;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneView _dangerZone;
        [SerializeField, Required, ChildGameObjectsOnly] private TokenController _resourcesTokenController;
        [SerializeField, Required, ChildGameObjectsOnly] private TokenController _hintsTokenController;

        public bool IsFree => Investigator == null;
        public Investigator Investigator { get; private set; }
        public TokenController ResourcesTokenController => _resourcesTokenController;
        public TokenController HintsTokenController => _hintsTokenController;

        /*******************************************************************/
        public void Init(Investigator investigator)
        {
            name = "InvestigatorZones" + investigator.InvestigatorCard.Info.Code;
            Investigator = investigator;
            _investigatorZone.Init(investigator.InvestigatorZone);
            _handZone.Init(investigator.HandZone);
            _deckZone.Init(investigator.DeckZone);
            _discardZone.Init(investigator.DiscardZone);
            _aidZone.Init(investigator.AidZone);
            _dangerZone.Init(investigator.DangerZone);
        }

        /*******************************************************************/
        public bool HasThisZone(ZoneView zoneView) =>
            zoneView == _investigatorZone
            || zoneView == _handZone
            || zoneView == _deckZone
            || zoneView == _discardZone
            || zoneView == _aidZone
            || zoneView == _dangerZone;
    }
}
