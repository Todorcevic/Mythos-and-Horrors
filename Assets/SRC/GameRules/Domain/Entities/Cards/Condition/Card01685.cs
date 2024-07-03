using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01685 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand | PlayActionType.Investigate;
        private IEnumerable<CardPlace> PlacesWithHints(Investigator investigator) =>
            investigator.CurrentPlace.ConnectedPlacesToMove.Append(investigator.CurrentPlace).Where(place => place.Hints.Value > 0);
        public override Func<Card> CardAffected => () => ControlOwner.CurrentPlace;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
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
                    .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Place");

                foreach (CardPlace place in PlacesWithHints(investigator))
                {
                    chooseHints.CreateEffect(place, new Stat(0, false), TakeHint, PlayActionType.Choose, investigator);

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

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!investigator.CurrentPlace.CanBeInvestigated.IsActive) return false;
            if (!PlacesWithHints(investigator).Any()) return false;
            return true;
        }
    }
}
