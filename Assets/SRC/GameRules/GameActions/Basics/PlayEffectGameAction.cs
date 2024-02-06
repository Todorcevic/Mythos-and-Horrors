using System;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayEffectGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public Effect Effect { get; }

        /*******************************************************************/
        public PlayEffectGameAction(Effect effect)
        {
            Effect = effect ?? throw new ArgumentNullException("Effect cant be null");
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _viewLayerProvider.PlayAnimationWith(this);
            await Effect.Logic();
        }
    }
}

