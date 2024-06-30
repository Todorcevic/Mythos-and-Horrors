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
            CreateBuff(CardToSelect, BuffOn, BuffOff);
            CreateActivation(1, Logic, Condition, PlayActionType.Elude);
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
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Place");
            interactableGameAction.CreateCancelMainButton();

            foreach (CardPlace place in investigator.CurrentPlace.ConnectedPlacesToMove)
            {
                interactableGameAction.CreateEffect(place, new Stat(0, false), MoveAndUnconfront, PlayActionType.Choose | PlayActionType.Move, playedBy: investigator);

                async Task MoveAndUnconfront() =>
                    await _gameActionsProvider.Create(new MoveInvestigatorAndUnconfrontGameAction(investigator, place));
            }

            await _gameActionsProvider.Create(new UpdateStatesGameAction(Exausted, true));
            await _gameActionsProvider.Create(interactableGameAction);
        }

        /*******************************************************************/
        private async Task BuffOn(IEnumerable<Card> cardsToBuff)
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(ControlOwner.Agility, 1));
        }

        private async Task BuffOff(IEnumerable<Card> cardsToDebuff)
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(ControlOwner.Agility, 1));
        }

        private IEnumerable<Card> CardToSelect() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
    }
}
