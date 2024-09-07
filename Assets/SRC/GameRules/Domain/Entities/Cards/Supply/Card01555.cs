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
            CreateBuff(CardToSelect, BuffOn, BuffOff, new Localization("Buff_Card01555"));
            CreateActivation(1, Logic, Condition, PlayActionType.Activate | PlayActionType.Move | PlayActionType.Elude, new Localization("Activation_Card01555"));
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (Exausted.IsActive) return false;
            if (investigator != ControlOwner) return false;
            if (investigator.CurrentPlace.ConnectedPlacesToMove.Count() == 0) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01555"));

            foreach (CardPlace place in investigator.CurrentPlace.ConnectedPlacesToMove)
            {
                interactableGameAction.CreateCardEffect(place, new Stat(0, false), MoveAndUnconfront,
                    PlayActionType.Choose, playedBy: investigator, new Localization("CardEffect_Card01555"), cardAffected: this);

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

        private IEnumerable<Card> CardToSelect() => IsInPlay.IsTrue ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
    }
}
