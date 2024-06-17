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
        protected override bool IsFast => false;
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand | PlayActionType.Investigate;
        private IEnumerable<CardPlace> PlacesWithHints(Investigator investigator) =>
            investigator.CurrentPlace.ConnectedPlacesToMove.Append(investigator.CurrentPlace).Where(place => place.Hints.Value > 0);

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            int amoutHintsLeft = 2;
            InvestigatePlaceGameAction investigate = new(investigator, investigator.CurrentPlace);
            investigate.SuccesEffects.Clear();
            investigate.SuccesEffects.Add(ChooseHints);

            await _gameActionsProvider.Create(investigate);

            /*******************************************************************/
            async Task ChooseHints()
            {
                InteractableGameAction chooseHints = new(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Place");

                chooseHints.CreateCancelMainButton();

                foreach (CardPlace place in PlacesWithHints(investigator))
                {
                    chooseHints.CreateEffect(place, new Stat(0, false), TakeHint, PlayActionType.Choose, investigator);

                    async Task TakeHint()
                    {
                        await _gameActionsProvider.Create(new GainHintGameAction(investigator, place.Hints, 1));
                        amoutHintsLeft--;
                        if (amoutHintsLeft > 0) await ChooseHints();
                    }
                }

                await _gameActionsProvider.Create(chooseHints);
            }
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (!PlacesWithHints(ControlOwner).Any()) return false;
            return true;
        }
    }
}
