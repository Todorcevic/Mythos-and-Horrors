using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01548 : CardSupply, IDamageable, IFearable
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

            CreateBuff(CardToBuff, BuffOn, BuffOff, "Buff_Card01548");
        }

        /*******************************************************************/
        private async Task BuffOff(IEnumerable<Card> allCards)
        {
            CardInvestigator investigatorCard = allCards.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(investigatorCard.MaxTurns, 1).Execute();
            if (investigatorCard.CurrentTurns.Value > investigatorCard.MaxTurns.Value)
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(investigatorCard.CurrentTurns, 1).Execute();
        }

        private async Task BuffOn(IEnumerable<Card> allCards)
        {
            CardInvestigator investigatorCard = allCards.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigatorCard.MaxTurns, 1).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigatorCard.CurrentTurns, 1).Execute();
        }

        private IEnumerable<Card> CardToBuff() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
    }
}
