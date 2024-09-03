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
            CreateActivation(1, TakeTomeOrSpellLogic, TakeTomeOrSpellCondition, PlayActionType.Activate, new Localization("Activation_Card01129"));
        }

        /*******************************************************************/
        private async Task TakeTomeOrSpellLogic(Investigator investigator)
        {
            IEnumerable<Card> cardToChoose = investigator.DeckZone.Cards.Skip(Math.Max(0, investigator.DeckZone.Cards.Count() - 6))
                .Take(6).Where(card => card.HasThisTag(Tag.Tome) || card.HasThisTag(Tag.Spell));
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01129"));
            foreach (Card card in cardToChoose)
            {
                interactableGameAction.CreateCardEffect(card, CreateStat(0), Take, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01129"));

                async Task Take()
                {
                    await _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, card).Execute();
                    await _gameActionsProvider.Create<HideCardsGameAction>().SetWith(cardToChoose.Except(new[] { card })).Execute();
                }
            }

            await _gameActionsProvider.Create<ShowCardsGameAction>().SetWith(cardToChoose).Execute();
            await interactableGameAction.Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(investigator.DeckZone).Execute();
        }

        private bool TakeTomeOrSpellCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            return true;
        }
    }
}
