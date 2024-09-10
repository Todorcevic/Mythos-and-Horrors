using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01540 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override bool IsFast => true;
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Tool };

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            CreateBuff(CardsToBuff, AddIntelligenceBuff, RemoveIntelligenceBuff, new Localization("Buff_Card01540"));
            CreateFastActivation(ReturnToHandLogic, ReturnToHandCondition, PlayActionType.Activate, new Localization("Activation_Card01540"));
        }

        /*******************************************************************/
        private bool ReturnToHandCondition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (ControlOwner != investigator) return false;
            if (CurrentPlace.Keys.Value > 0) return false;
            return true;
        }

        private async Task ReturnToHandLogic(Investigator investigator) =>
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(this, investigator.HandZone).Execute();


        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff()
        {
            return Condition() ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();

            /*******************************************************************/
            bool Condition()
            {
                if (IsInPlay.IsFalse) return false;
                if (ControlOwner.IsInvestigating.IsFalse) return false;
                return true;
            }
        }
        private async Task AddIntelligenceBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.OfType<CardInvestigator>().ToDictionary(card => card.Intelligence, card => 1);
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(map).Execute();
        }

        private async Task RemoveIntelligenceBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.OfType<CardInvestigator>().ToDictionary(card => card.Intelligence, card => 1);
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(map).Execute();
        }
    }
}
