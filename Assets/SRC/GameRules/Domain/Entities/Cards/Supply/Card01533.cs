using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01533 : CardSupply, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Miskatonic };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);
            CreateBuff(CardsToBuff, AddIntelligenceBuff, RemoveIntelligenceBuff);
            CreateOptativeReaction<InvestigatePlaceGameAction>(Condition, Logic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task Logic(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            await _gameActionsProvider.Create(new GainResourceGameAction(ControlOwner, 1));
        }

        private bool Condition(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            if (!IsInPlay) return false;
            if (investigatePlaceGameAction.ActiveInvestigator != ControlOwner) return false;
            if (!investigatePlaceGameAction.IsSucceed) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RemoveIntelligenceBuff(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create(new DecrementStatGameAction(cardInvestigator.Intelligence, 1));
        }

        private async Task AddIntelligenceBuff(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create(new IncrementStatGameAction(cardInvestigator.Intelligence, 1));
        }

        private IEnumerable<Card> CardsToBuff() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : new Card[0];

        /*******************************************************************/
    }
}
