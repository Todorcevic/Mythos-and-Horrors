using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;
using ModestTree;

namespace MythosAndHorrors.GameRules
{
    public class Card01127 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Dictionary<Investigator, State> InvestigatorsUsed { get; } = new();
        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            _investigatorsProvider.AllInvestigators.ForEach(investigator => InvestigatorsUsed.Add(investigator, CreateState(false)));
            CreateActivation(1, TakeSupportLogic, TakeSupportCondition, PlayActionType.Activate, new Localization("Activation_Card01127"));
        }

        /*******************************************************************/
        private async Task TakeSupportLogic(Investigator investigator)
        {
            IEnumerable<CardSupply> supportsInDeck = investigator.DeckZone.Cards.OfType<CardSupply>().Where(card => card.HasThisTag(Tag.Ally));
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01127"));

            CardSupply cardSelected = null;
            foreach (CardSupply cardSupply in supportsInDeck)
            {
                interactableGameAction.CreateCardEffect(cardSupply, CreateStat(0), Take, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01127"));

                async Task Take()
                {
                    cardSelected = cardSupply;
                    await Task.CompletedTask;
                }
            }

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(InvestigatorsUsed[investigator], true).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supportsInDeck, _chaptersProvider.CurrentScene.LimboZone).Execute();
            await interactableGameAction.Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supportsInDeck.Except(cardSelected), investigator.DeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(investigator.DeckZone).Execute();
            if (cardSelected != null) await _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardSelected).Execute();
        }

        private bool TakeSupportCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            if (InvestigatorsUsed[investigator].IsActive) return false;
            return true;
        }
    }
}
