using ModestTree;
using System;
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
            CreateBuff(CardsToBuff, GainPowerActivationLogic, GainPowerDeactivationLogic);
            CreateFastActivation(Logic, Condition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
            await _gameActionsProvider.Create(new IncrementStatGameAction(investigator.CurrentTurns, 2));
        }

        /*******************************************************************/
        private async Task GainPowerDeactivationLogic(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create(new DecrementStatGameAction(cardInvestigator.Strength, 1));
        }

        private async Task GainPowerActivationLogic(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create(new IncrementStatGameAction(cardInvestigator.Strength, 1));
        }

        private IEnumerable<Card> CardsToBuff() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : new Card[0];

        /*******************************************************************/
    }
}
