using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01527 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateBuff(CardsToBuff, GainPowerActivationLogic, GainPowerDeactivationLogic, new Localization("Buff_Card01527"));
            CreateFastActivation(Logic, Condition, PlayActionType.Activate, new Localization("Activation_Card01527"));
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay.IsTrue) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigator.CurrentTurns, 2).Execute();
        }

        /*******************************************************************/
        private async Task GainPowerDeactivationLogic(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardInvestigator.Power, 1).Execute();
        }

        private async Task GainPowerActivationLogic(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(cardInvestigator.Power, 1).Execute();
        }

        private IEnumerable<Card> CardsToBuff() => IsInPlay.IsTrue ? new[] { ControlOwner.InvestigatorCard } : new Card[0];

        /*******************************************************************/
    }
}
