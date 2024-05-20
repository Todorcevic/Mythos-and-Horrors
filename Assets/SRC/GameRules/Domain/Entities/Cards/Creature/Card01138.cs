using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01138 : CardCreature
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;
        public CardPlace SpawnPlace => SceneCORE2.Graveyard;
        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(CreateStat(1), ParleyActivate, ParleyConditionToActivate, withOpportunityAttck: false);
        }

        /*******************************************************************/
        private bool ParleyConditionToActivate(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (CurrentPlace != investigator.CurrentPlace) return false;
            if (investigator.HandZone.Cards.Count(card => card.CanBeDiscarded) < 4) return false;
            return true;
        }

        private Stat AmountDiscarded { get; }

        private async Task ParleyActivate(Investigator investigator)
        {
            await _gameActionsProvider.Create(new ParleyGameAction(PayCreature));

            /*******************************************************************/
            async Task PayCreature()
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Parlay", investigator);
                foreach (Card card in investigator.HandZone.Cards)
                {
                    interactableGameAction.Create()
                        .SetCard(card)
                        .SetInvestigator(investigator)
                        .SetCardAffected(this)
                        .SetLogic(Discard);

                    /*******************************************************************/
                    async Task Discard()
                    {
                        await _gameActionsProvider.Create(new DiscardGameAction(card));
                        await _gameActionsProvider.Create(new IncrementStatGameAction(AmountDiscarded, 1));
                        if (AmountDiscarded.Value == 4) await _gameActionsProvider.Create(new DiscardGameAction(this));
                        else await PayCreature();
                    }
                }

                await _gameActionsProvider.Create(interactableGameAction);
            }
        }
    }
}
