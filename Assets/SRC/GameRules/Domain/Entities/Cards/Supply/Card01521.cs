using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01521 : CardSupply, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Creature };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);

            CreateOptativeReaction<HarmToCardGameAction>(Condition, Logic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task Logic(HarmToCardGameAction harmToCardGameAction)
        {
            await _gameActionsProvider.Create(new HarmToCardGameAction(harmToCardGameAction.ByThisCard, this, amountDamage: 1));
        }

        private bool Condition(HarmToCardGameAction harmToCardGameAction)
        {
            if (!IsInPlay) return false;
            if (harmToCardGameAction.Card != this) return false;
            if (harmToCardGameAction.ByThisCard is not CardCreature) return false;
            return true;
        }
    }
}
