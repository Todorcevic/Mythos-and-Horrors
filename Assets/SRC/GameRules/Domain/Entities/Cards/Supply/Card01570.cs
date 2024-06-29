using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01570 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Slot ExtraMagicalSlot { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, Logic, Condition, PlayActionType.Activate);
            CreateBuff(CardsToBuff, ActivationLogic, Deactivationlogic);
            ExtraMagicalSlot = new Slot(SlotType.Magical);
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff()
        {
            return IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
        }

        private async Task ActivationLogic(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create(new AddSlotGameAction(ControlOwner, ExtraMagicalSlot));
        }

        private async Task Deactivationlogic(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create(new RemoveSlotGameAction(ControlOwner, ExtraMagicalSlot));
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (Exausted.IsActive) return false;
            if (!investigator.CardsInPlay.OfType<IChargeable>().Any()) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose spell");
            interactableGameAction.CreateCancelMainButton(_gameActionsProvider);

            foreach (Card card in investigator.CardsInPlay.Where(card => card is IChargeable chargeable && chargeable.Charge.ChargeType == ChargeType.MagicCharge))
            {
                interactableGameAction.CreateEffect(card, new Stat(0, false), SelectSpell, PlayActionType.Choose, investigator);

                /*******************************************************************/
                async Task SelectSpell() =>
                    await _gameActionsProvider.Create(new IncrementStatGameAction(((IChargeable)card).Charge.Amount, 1));
            }

            await _gameActionsProvider.Create(new UpdateStatesGameAction(Exausted, true));
            await _gameActionsProvider.Create(interactableGameAction);
        }

        /*******************************************************************/
    }
}
