﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionReaction : CardCondition
    {
        protected abstract Localization Localization { get; }
        protected abstract GameActionTime FastReactionAtStart { get; }
        public GameConditionWith<GameAction> PlayFromHandCondition { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandCondition = new GameConditionWith<GameAction>(CanPlayFromHandWith);
            CreateFastPlayCondition<GameAction>();
        }

        /*******************************************************************/
        private OptativeReaction<T> CreateFastPlayCondition<T>() where T : GameAction
        {
            Func<T, bool> condition = PlayFromHandCondition.IsTrueWith;
            Func<T, Task> logic = PlayFromHandCommand.RunWith;

            return CreateOptativeReaction(condition, logic, FastReactionAtStart, Localization, PlayFromHandActionType);
        }

        /*******************************************************************/
        protected bool CanPlayFromHandWith(GameAction gameAction)
        {
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            return CanPlayFromHandSpecific(gameAction);
        }

        protected abstract bool CanPlayFromHandSpecific(GameAction gameAction);
    }
}
