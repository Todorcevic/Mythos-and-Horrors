using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01550 : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        protected override bool IsFast => true;

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (ControlOwner.CurrentPlace.ConnectedPlacesToMove.All(place => place.CreaturesInThisPlace.Any())) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            IEnumerable<CardPlace> connectedPlacesToMove = investigator.CurrentPlace.ConnectedPlacesToMove.Where(place => !place.CreaturesInThisPlace.Any());

            InteractableGameAction interactable = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Place To move", investigator);
            interactable.CreateCancelMainButton();

            foreach (CardPlace place in connectedPlacesToMove)
            {
                interactable.CreateEffect(place, new Stat(0, false), DeconfrontAndMove, PlayActionType.Choose, investigator);

                async Task DeconfrontAndMove() => await _gameActionsProvider.Create(new MoveInvestigatorAndUnconfront(investigator, place));
            }

            await _gameActionsProvider.Create(interactable);
        }
    }
}
