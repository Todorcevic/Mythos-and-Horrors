using ModestTree;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01554 : CardSupply, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Criminal };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);

            CreateBuff(CardToBuff, BuffOn, BuffOff);
        }

        /*******************************************************************/
        private async Task BuffOff(IEnumerable<Card> allCards)
        {
            CardInvestigator investigatorCard = allCards.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create(new DecrementStatGameAction(investigatorCard.MaxTurns, 1));
            if (investigatorCard.CurrentTurns.Value > investigatorCard.MaxTurns.Value)
                await _gameActionsProvider.Create(new DecrementStatGameAction(investigatorCard.CurrentTurns, 1));
        }

        private async Task BuffOn(IEnumerable<Card> allCards)
        {
            CardInvestigator investigatorCard = allCards.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create(new IncrementStatGameAction(investigatorCard.MaxTurns, 1));
            await _gameActionsProvider.Create(new IncrementStatGameAction(investigatorCard.CurrentTurns, 1));
        }

        private IEnumerable<Card> CardToBuff() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
    }
}
