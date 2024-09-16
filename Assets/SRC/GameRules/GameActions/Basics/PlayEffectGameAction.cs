using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PlayEffectGameAction : GameAction
    {
        public BaseEffect Effect { get; private set; }

        /*******************************************************************/
        public PlayEffectGameAction SetWith(BaseEffect effect)
        {
            Effect = effect ?? throw new ArgumentNullException("Effect cant be null");
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await Effect.Logic();
        }
    }
}

