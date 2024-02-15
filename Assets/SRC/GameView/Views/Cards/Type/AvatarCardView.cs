using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class AvatarCardView : CardView
    {
        [Title("AvatarCardView")]
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _health;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private TurnController _turnController;

        public Investigator Investigator => Card.Owner;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            _health.SetStat(Investigator.InvestigatorCard.Health);
            _sanity.SetStat(Investigator.InvestigatorCard.Sanity);
            _turnController.Init(Investigator.InvestigatorCard.Turns);
        }
        /*******************************************************************/
        public override void UpdateState() { }
    }
}
