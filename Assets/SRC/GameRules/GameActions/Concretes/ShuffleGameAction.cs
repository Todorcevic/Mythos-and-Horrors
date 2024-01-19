using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ShuffleGameAction : GameAction
    {
        [Inject] private readonly IAnimator _animator;

        public Zone ZoneToShuffle { get; private set; }

        /*******************************************************************/
        public async Task Run(Zone zoneToShuffle)
        {
            ZoneToShuffle = zoneToShuffle;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ZoneToShuffle.Cards.Shuffle();
            await _animator.PlayAnimationWith(this);
        }
    }
}
