using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class AvatarCardView : CardView
    {
        [Title(nameof(AvatarCardView))]
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _health;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private TurnController _turnController;

        public Investigator Investigator => Card.Owner;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            _health.SetStat(Investigator.Health);
            _sanity.SetStat(Investigator.Sanity);
            _turnController.Init(Investigator.Turns);
        }
    }
}
