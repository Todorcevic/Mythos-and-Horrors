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

        protected override bool IsFast => true;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            InteractableGameAction interactable = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator");
            interactable.CreateCancelMainButton();

            foreach (Investigator investigatorToChoose in investigator.CurrentPlace.InvestigatorsInThisPlace)
            {
                interactable.CreateEffect(investigatorToChoose.AvatarCard, new Stat(0, false), Draw, PlayActionType.Choose, investigator);

                async Task Draw()
                {
                    await _gameActionsProvider.Create(new DrawAidGameAction(investigatorToChoose));
                    await _gameActionsProvider.Create(new DrawAidGameAction(investigatorToChoose));
                    await _gameActionsProvider.Create(new DrawAidGameAction(investigatorToChoose));
                }
            }

            await _gameActionsProvider.Create(interactable);
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;

    }
}
