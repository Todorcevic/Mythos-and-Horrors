using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveInvestigatorGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _animator;

        public Investigator Investigator { get; }
        public Zone Zone { get; }

        /*******************************************************************/
        public MoveInvestigatorGameAction(Investigator investigator, Zone zone)
        {
            Investigator = investigator;
            Zone = zone;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.AvatarCard.MoveToZone(Zone);
            await _animator.PlayAnimationWith(this);
        }
    }
}
