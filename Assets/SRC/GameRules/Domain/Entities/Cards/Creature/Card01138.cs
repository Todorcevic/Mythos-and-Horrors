using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01138 : CardCreature, ISpawnable
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
            CreateActivation(CreateStat(1), ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley);
        }

        /*******************************************************************/
        private bool ParleyConditionToActivate(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (CurrentPlace != investigator.CurrentPlace) return false;
            if (investigator.DiscardableCardsInHand.Count() < 4) return false;
            return true;
        }

        private Stat _amountDiscarded;

        private async Task ParleyActivate(Investigator investigator)
        {
            _amountDiscarded = CreateStat(0);
            await _gameActionsProvider.Create(new ParleyGameAction(PayCreature));

            /*******************************************************************/
            async Task PayCreature()
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Parlay");
                foreach (Card card in investigator.HandZone.Cards.Where(card => card.CanBeDiscarded))
                {
                    interactableGameAction.CreateEffect(card,
                        new Stat(0, false),
                        Discard,
                        PlayActionType.Choose,
                        playedBy: investigator,
                        cardAffected: this);

                    /*******************************************************************/
                    async Task Discard()
                    {
                        await _gameActionsProvider.Create(new DiscardGameAction(card));
                        await _gameActionsProvider.Create(new IncrementStatGameAction(_amountDiscarded, 1));
                        if (_amountDiscarded.Value == 4)
                            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.VictoryZone));
                        else await PayCreature();
                    }
                }

                await _gameActionsProvider.Create(interactableGameAction);
            }
        }
    }
}
