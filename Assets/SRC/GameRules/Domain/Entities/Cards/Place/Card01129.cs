using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;
using ModestTree;

namespace MythosAndHorrors.GameRules
{
    public class Card01129 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

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

            Card cardSelected = null;
            foreach (Card card in cardToChoose)
            {
                interactableGameAction.CreateCardEffect(card, CreateStat(0), Take, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01129"));

                /*******************************************************************/
                async Task Take()
                {
                    cardSelected = card;
                    await Task.CompletedTask;
                }
            }

            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardToChoose, _chaptersProvider.CurrentScene.LimboZone).Execute();
            await interactableGameAction.Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardToChoose.Except(cardSelected), investigator.DeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(investigator.DeckZone).Execute();
            if (cardSelected != null) await _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardSelected).Execute();
        }

        private bool TakeTomeOrSpellCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            return true;
        }
    }
}
