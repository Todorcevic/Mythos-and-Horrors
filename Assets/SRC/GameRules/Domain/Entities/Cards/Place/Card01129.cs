using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01129 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, TakeTomeOrSpellLogic, TakeTomeOrSpellCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private async Task TakeTomeOrSpellLogic(Investigator investigator)
        {
            IEnumerable<Card> supportsInDeck = investigator.DeckZone.Cards.Skip(Math.Max(0, investigator.DeckZone.Cards.Count() - 6)).Take(6).Where(card => card.HasThisTag(Tag.Tome) || card.HasThisTag(Tag.Spell));
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Take support");
            foreach (CardSupply cardSupply in supportsInDeck)
            {
                interactableGameAction.CreateEffect(cardSupply, CreateStat(0), Take, PlayActionType.Choose, investigator);

                async Task Take()
                {
                    await _gameActionsProvider.Create(new DrawGameAction(investigator, cardSupply));
                    await _gameActionsProvider.Create(new HideCardsGameAction(supportsInDeck.Except(new[] { cardSupply })));
                }
            }

            await _gameActionsProvider.Create(new ShowCardsGameAction(supportsInDeck));
            await interactableGameAction.Start();
            await _gameActionsProvider.Create(new ShuffleGameAction(investigator.DeckZone));
        }

        private bool TakeTomeOrSpellCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            return true;
        }
    }
}
