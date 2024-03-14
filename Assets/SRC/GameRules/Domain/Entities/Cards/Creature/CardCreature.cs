using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardCreature : Card
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public Stat Health { get; private set; }
        public Stat Strength { get; private set; }
        public Stat Agility { get; private set; }
        public Stat Damage { get; private set; }
        public Stat Fear { get; private set; }
        public Stat InvestigatorAttackTurnsCost { get; private set; }
        public Stat InvestigatorConfronTurnsCost { get; private set; }
        public Stat EludeTurnsCost { get; private set; }

        /*******************************************************************/
        public int TotalEnemyHits => (Info.CreatureDamage ?? 0) + (Info.CreatureFear ?? 0);
        public bool IsConfronted => ConfrontedInvestigator != null;
        public Investigator ConfrontedInvestigator
        {
            get
            {
                Investigator investigator = _investigatorProvider.GetInvestigatorWithThisZone(CurrentZone);
                return investigator?.DangerZone == CurrentZone ? investigator : null;
            }
        }

        public CardPlace CurrentPlace => _cardsProvider.GetCardWithThisZone(CurrentZone) as CardPlace ?? ConfrontedInvestigator?.CurrentPlace;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = new Stat(Info.Health ?? 0, Info.Health ?? 0);
            Strength = new Stat(Info.Strength ?? 0);
            Agility = new Stat(Info.Agility ?? 0);
            Damage = new Stat(Info.CreatureDamage ?? 0);
            Fear = new Stat(Info.CreatureFear ?? 0);
            InvestigatorAttackTurnsCost = new Stat(1, 1);
            InvestigatorConfronTurnsCost = new Stat(1, 1);
            EludeTurnsCost = new Stat(1, 1);
        }
    }
}
