using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01137 : CardCreature, ISpawnable
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;
        public CardPlace SpawnPlace => SceneCORE2.Center;
        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<CreatureAttackGameAction>(HealCondition, HealLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task HealLogic(CreatureAttackGameAction creatureAttackGameAction)
        {
            await _gameActionsProvider.Create<HealthGameAction>().SetWith(this, amountDamageToRecovery: 1).Start();
        }

        private bool HealCondition(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (creatureAttackGameAction.Creature != this) return false;
            if (!IsInPlay) return false;
            if (DamageRecived.Value <= 0) return false;
            return true;
        }

    }
}
