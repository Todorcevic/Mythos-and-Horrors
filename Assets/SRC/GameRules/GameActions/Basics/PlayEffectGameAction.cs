﻿using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PlayEffectGameAction : GameAction
    {
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
        }
    }
}
