using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01572 : CardSupply, IDamageable
    {
        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Armor };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
        }

        /*******************************************************************/
    }
}
