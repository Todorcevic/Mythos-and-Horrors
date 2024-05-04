using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01596 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Flaw };
        public override Zone ZoneToMove => _chaptersProvider.CurrentScene.LimboZone;

        /*******************************************************************/
        protected override async Task ObligationLogic()
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Card");
            foreach (Card card in Owner.HandZone.Cards.Where(card => card.CanDiscard))
            {
                interactableGameAction.Create()
                            .SetCard(card)
                            .SetInvestigator(Owner)
                            .SetCardAffected(this)
                            .SetLogic(Discard);

                async Task Discard()
                {
                    await _gameActionsProvider.Create(new SafeForeach<Card>(Owner.HandZone.Cards.Where(card => card.CanDiscard).Except(new[] { card }), Logic));

                    async Task Logic(Card card) => await _gameActionsProvider.Create(new DiscardGameAction(card));
                }
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
