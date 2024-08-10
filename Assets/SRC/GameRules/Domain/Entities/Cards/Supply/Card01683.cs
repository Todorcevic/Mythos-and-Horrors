using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01683 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent, Tag.Science };
        public Stat AmountSupplies { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AmountSupplies = CreateStat(4);
            CreateActivation(1, HealthActivate, HealConditionToActivate, PlayActionType.Activate);
            CreateForceReaction<UpdateStatGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task DiscardLogic(UpdateStatGameAction updateStatGameAction)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        private bool DiscardCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!IsInPlay) return false;
            if (!updateStatGameAction.HasThisStat(AmountSupplies)) return false;
            if (AmountSupplies.Value > 0) return false;
            return true;
        }

        private IEnumerable<Card> CardsToHealth(CardPlace place) => place.InvestigatorsInThisPlace
                .Where(investigator => investigator.CanBeHealed || investigator.CanBeRestoreSanity).Select(investigator => investigator.InvestigatorCard)
            .Concat(Other(place));

        private IEnumerable<Card> Other(CardPlace place) => _cardsProvider.GetCards<CardSupply>()
           .Where(card => card.IsInPlay && card.HasThisTag(Tag.Ally) && card.CurrentPlace == place
                 && ((card is IDamageable damageable && damageable.CanBeHealed) || (card is IFearable fearable && fearable.CanBeRestoreSanity)));

        /*******************************************************************/
        public async Task HealthActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(AmountSupplies, 1).Execute();

            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01683");

            foreach (Card card in CardsToHealth(activeInvestigator.CurrentPlace))
            {
                interactableGameAction.CreateEffect(card,
                    new Stat(0, false),
                    RestoreHealthAndFearInvestigator,
                    PlayActionType.Choose,
                    playedBy: activeInvestigator);

                /*******************************************************************/
                async Task RestoreHealthAndFearInvestigator() => await _gameActionsProvider.Create<HealthGameAction>().SetWith(card, amountDamageToRecovery: 1, amountFearToRecovery: 1).Execute();
            }

            await interactableGameAction.Execute();
        }

        public bool HealConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != activeInvestigator) return false;
            if (!CardsToHealth(activeInvestigator.CurrentPlace).Any()) return false;
            return true;
        }
    }
}
