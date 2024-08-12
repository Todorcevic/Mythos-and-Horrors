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

        public Stat DiscardRemaining { get; private set; }
        private SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;
        public CardPlace SpawnPlace => SceneCORE2.Graveyard;
        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley, "Activation_Card01138");
        }

        /*******************************************************************/
        private bool ParleyConditionToActivate(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (CurrentPlace != investigator.CurrentPlace) return false;
            if (investigator.DiscardableCardsInHand.Count() < 4) return false;
            return true;
        }

        private async Task ParleyActivate(Investigator investigator)
        {
            DiscardRemaining = CreateStat(4);
            await _gameActionsProvider.Create<ParleyGameAction>().SetWith(PayCreature).Execute();

            /*******************************************************************/
            async Task PayCreature()
            {
                while (DiscardRemaining.Value > 0)
                {
                    await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(DiscardRemaining, 1).Execute();
                    InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                        .SetWith(canBackToThisInteractable: false, mustShowInCenter: false, "Interactable_Card01138", DescriptionParams());
                    foreach (Card card in investigator.HandZone.Cards.Where(card => card.CanBeDiscarted.IsActive))
                    {
                        interactableGameAction.CreateEffect(card,
                            new Stat(0, false),
                            Discard,
                            PlayActionType.Choose,
                            playedBy: investigator,
                            localizableCode: "CardEffect_Card01138",
                            cardAffected: this);

                        /*******************************************************************/
                        async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                    }

                    await interactableGameAction.Execute();
                }

                await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(this, _chaptersProvider.CurrentScene.VictoryZone).Execute();

                /*******************************************************************/
                string DescriptionParams() => (DiscardRemaining.Value + 1).ToString();
            }
        }
    }
}
