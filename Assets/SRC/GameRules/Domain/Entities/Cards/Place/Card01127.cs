using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01127 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Dictionary<Investigator, State> InvestigatorsUsed { get; } = new();
        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            _investigatorsProvider.AllInvestigators.ForEach(investigator => InvestigatorsUsed.Add(investigator, CreateState(false)));
            CreateActivation(1, TakeSupportLogic, TakeSupportCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private async Task TakeSupportLogic(Investigator investigator)
        {
            IEnumerable<CardSupply> supportsInDeck = investigator.DeckZone.Cards.OfType<CardSupply>().Where(card => card.HasThisTag(Tag.Ally));
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01127");
            foreach (CardSupply cardSupply in supportsInDeck)
            {
                interactableGameAction.CreateEffect(cardSupply, CreateStat(0), Take, PlayActionType.Choose, investigator);

                async Task Take()
                {
                    await _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardSupply).Execute();
                    await _gameActionsProvider.Create<HideCardsGameAction>().SetWith(supportsInDeck.Except(new[] { cardSupply })).Execute();
                }
            }

            await _gameActionsProvider.Create<ShowCardsGameAction>().SetWith(supportsInDeck).Execute();
            await interactableGameAction.Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(investigator.DeckZone).Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(InvestigatorsUsed[investigator], true).Execute();
        }

        private bool TakeSupportCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            if (InvestigatorsUsed[investigator].IsActive) return false;
            return true;
        }
    }
}
