﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01550 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override bool IsFast => true;
        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (ControlOwner.CurrentPlace.ConnectedPlacesToMove.All(place => place.CreaturesInThisPlace.Any())) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            IEnumerable<CardPlace> connectedPlacesToMove = investigator.CurrentPlace.ConnectedPlacesToMove.Where(place => !place.CreaturesInThisPlace.Any());
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01550"));

            foreach (CardPlace place in connectedPlacesToMove)
            {
                interactable.CreateCardEffect(place, new Stat(0, false), DeconfrontAndMove, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01550"));

                /*******************************************************************/
                async Task DeconfrontAndMove() => await _gameActionsProvider.Create<MoveInvestigatorAndUnconfrontGameAction>()
                    .SetWith(investigator, place).Execute();
            }

            await interactable.Execute();
        }
    }
}
