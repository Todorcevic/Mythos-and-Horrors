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
            CreateBuff(CardsToBuff, AddIntelligenceBuff, RemoveIntelligenceBuff, new Localization("Buff_Card01533"));
            CreateOptativeReaction<InvestigatePlaceGameAction>(Condition, Logic, GameActionTime.After, new Localization("OptativeReaction_Card01533"));
        }

        /*******************************************************************/
        private async Task Logic(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(ControlOwner, 1).Execute();
        }

        private bool Condition(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (investigatePlaceGameAction.ActiveInvestigator != ControlOwner) return false;
            if (!investigatePlaceGameAction.IsSucceed) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RemoveIntelligenceBuff(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardInvestigator.Intelligence, 1).Execute();
        }

        private async Task AddIntelligenceBuff(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(cardInvestigator.Intelligence, 1).Execute();
        }

        private IEnumerable<Card> CardsToBuff() => IsInPlay.IsTrue ? new[] { ControlOwner.InvestigatorCard } : new Card[0];

        /*******************************************************************/
    }
}
