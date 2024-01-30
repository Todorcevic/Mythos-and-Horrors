using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ShuffleGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _animator;

        public Zone ZoneToShuffle { get; }

        /*******************************************************************/
        public ShuffleGameAction(Zone zoneToShuffle)
        {
            ZoneToShuffle = zoneToShuffle;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ZoneToShuffle.Cards.Shuffle();
            await _animator.PlayAnimationWith(this);
        }
    }
}
