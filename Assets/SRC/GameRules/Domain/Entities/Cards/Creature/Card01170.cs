﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01170 : CardCreature, ICounterAttackable, ISpawnable
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist };
        public CardPlace SpawnPlace => _chaptersProvider.CurrentScene.PlaceCards.FirstOrDefault(place => place.IsInPlay.IsTrue && place.IsAlone);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<ScenePhaseGameAction>(TakeEldrichCondition, TakeEldrichLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task TakeEldrichLogic(ScenePhaseGameAction action)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(Eldritch, 1).Execute();
        }

        private bool TakeEldrichCondition(ScenePhaseGameAction action)
        {
            if (IsInPlay.IsFalse) return false;
            return true;
        }
    }

}
