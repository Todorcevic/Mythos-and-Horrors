using System;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayEffectGameAction : GameAction
    {
        [Inject] private readonly IPresenter<PlayEffectGameAction> _rotateCardPresenter;

        public Effect Effect { get; }
        public bool IsEffectPlayed { get; private set; }

        /*******************************************************************/
        public PlayEffectGameAction(Effect effect)
        {
            Effect = effect ?? throw new ArgumentNullException("Effect cant be null");
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _rotateCardPresenter.PlayAnimationWith(this);
            await Effect.Logic();
            IsEffectPlayed = true;
            await _rotateCardPresenter.PlayAnimationWith(this);
        }
    }
}

