using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01555 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Criminal };

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            CreateBuff(CardToSelect, BuffOn, BuffOff, "Buff_Card01555");
            CreateFastActivation(Logic, Condition, PlayActionType.Activate, "Activation_Card01555");
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (Exausted.IsActive) return false;
            if (investigator != ControlOwner) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01555");

            foreach (CardPlace place in investigator.CurrentPlace.ConnectedPlacesToMove)
            {
                interactableGameAction.CreateEffect(place, place.MoveTurnsCost, MoveAndUnconfront,
                    PlayActionType.Move | PlayActionType.Elude, playedBy: investigator, "CardEffect_Card01555", cardAffected: this);

                /*******************************************************************/
                async Task MoveAndUnconfront() =>
                    await _gameActionsProvider.Create<MoveInvestigatorAndUnconfrontGameAction>().SetWith(investigator, place).Execute();
            }

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
            await interactableGameAction.Execute();
        }

        /*******************************************************************/
        private async Task BuffOn(IEnumerable<Card> cardsToBuff)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(ControlOwner.Agility, 1).Execute();
        }

        private async Task BuffOff(IEnumerable<Card> cardsToDebuff)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(ControlOwner.Agility, 1).Execute();
        }

        private IEnumerable<Card> CardToSelect() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
    }
}
