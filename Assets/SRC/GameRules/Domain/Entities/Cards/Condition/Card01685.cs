using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01685 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand;
        private IEnumerable<CardPlace> PlacesWithHints(Investigator investigator) =>
            investigator.CurrentPlace.ConnectedPlacesToMove.Append(investigator.CurrentPlace).Where(place => place.Hints.Value > 0);
        public override Func<Card> CardAffected => () => ControlOwner.CurrentPlace;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandTurnsCost = CreateStat(0);
        }

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01685"));

            interactable.CreateCardEffect(investigator.CurrentPlace, investigator.CurrentPlace.InvestigationTurnsCost, Investigate, PlayActionType.Investigate, investigator, new Localization("CardEffect_Card01685"), cardAffected: this);

            await interactable.Execute();

            /*******************************************************************/
            async Task Investigate()
            {
                int amoutHintsLeft = 2;
                InvestigatePlaceGameAction investigate = _gameActionsProvider.Create<InvestigatePlaceGameAction>()
                    .SetWith(investigator, investigator.CurrentPlace);
                investigate.SuccesEffects.Clear();
                investigate.SuccesEffects.Add(ChooseHints);
                await investigate.Execute();

                /*******************************************************************/
                async Task ChooseHints()
                {
                    InteractableGameAction chooseHints = _gameActionsProvider.Create<InteractableGameAction>()
                        .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01685-2"));

                    foreach (CardPlace place in PlacesWithHints(investigator))
                    {
                        chooseHints.CreateCardEffect(place, new Stat(0, false), TakeHint, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01685-1"));

                        async Task TakeHint()
                        {
                            await _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, place.Hints, 1).Execute();
                            amoutHintsLeft--;
                            if (amoutHintsLeft > 0) await ChooseHints();
                        }
                    }
                    await chooseHints.Execute();
                }
            }
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!investigator.CurrentPlace.CanBeInvestigated.IsTrue) return false;
            if (!investigator.CanInvestigate.IsTrue) return false;
            if (!PlacesWithHints(investigator).Any()) return false;
            return true;
        }
    }
}
