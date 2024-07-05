using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01543 : CardConditionPlayFromHand
    {
        [Inject] private readonly InvestigatorsProvider investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator");

            foreach (Investigator investigatorToChoose in investigator.CurrentPlace.InvestigatorsInThisPlace)
            {
                interactable.CreateEffect(investigatorToChoose.AvatarCard, new Stat(0, false), Draw, PlayActionType.Choose, investigator);

                /*******************************************************************/
                async Task Draw() => await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigatorToChoose, 3).Execute();
            }
            await interactable.Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator) => true;
    }
}
