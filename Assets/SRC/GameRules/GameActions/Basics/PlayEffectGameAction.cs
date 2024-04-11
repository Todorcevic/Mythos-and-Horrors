using System;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayEffectGameAction : GameAction
    {
        [Inject] private readonly IPresenter<PlayEffectGameAction> _playEffectPresenter;

        public Effect Effect { get; }

        /*******************************************************************/
        public PlayEffectGameAction(Effect effect)
        {
            Effect = effect ?? throw new ArgumentNullException("Effect cant be null");
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await Effect.Logic();
            await _playEffectPresenter.PlayAnimationWith(this);
        }
    }
}

