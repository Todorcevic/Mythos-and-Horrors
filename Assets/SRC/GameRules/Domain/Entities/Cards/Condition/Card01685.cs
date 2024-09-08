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
        private IEnumerable<CardPlace> PlacesWithKeys(Investigator investigator) =>
            investigator.CurrentPlace.ConnectedPlacesToMove.Append(investigator.CurrentPlace).Where(place => place.Keys.Value > 0);
        public override Func<Card> CardAffected => () => ControlOwner.CurrentPlace;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01685"));

            interactable.CreateCardEffect(investigator.CurrentPlace, new Stat(0, false), Investigate, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01685"), cardAffected: this);

            await interactable.Execute();

            /*******************************************************************/
            async Task Investigate()
            {
                int amoutKeysLeft = 2;
                InvestigatePlaceGameAction investigate = _gameActionsProvider.Create<InvestigatePlaceGameAction>()
                    .SetWith(investigator, investigator.CurrentPlace);
                investigate.SuccesEffects.Clear();
                investigate.SuccesEffects.Add(ChooseKeys);
                await investigate.Execute();

                /*******************************************************************/
                async Task ChooseKeys()
                {
                    InteractableGameAction chooseKeys = _gameActionsProvider.Create<InteractableGameAction>()
                        .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01685-2"));

                    foreach (CardPlace place in PlacesWithKeys(investigator))
                    {
                        chooseKeys.CreateCardEffect(place, new Stat(0, false), TakeKey, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01685-1"));

                        async Task TakeKey()
                        {
                            await _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, place.Keys, 1).Execute();
                            amoutKeysLeft--;
                            if (amoutKeysLeft > 0) await ChooseKeys();
                        }
                    }
                    await chooseKeys.Execute();
                }
            }
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (investigator.CurrentPlace.CanBeInvestigated.IsFalse) return false;
            if (!PlacesWithKeys(investigator).Any()) return false;
            return true;
        }
    }
}
