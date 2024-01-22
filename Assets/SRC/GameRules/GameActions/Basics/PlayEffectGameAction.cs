using System;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayEffectGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _animator;

        public Effect Effect { get; private set; }

        /*******************************************************************/
        public async Task Run(Effect effect)
        {
            Effect = effect ?? throw new ArgumentNullException("Effect cant be null");
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _animator.PlayAnimationWith(this);
            await Effect.Logic();
        }
    }
}

