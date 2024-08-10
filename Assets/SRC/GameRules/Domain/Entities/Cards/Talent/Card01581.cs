using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01581 : CardTalent
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Innate };

        /*******************************************************************/
        public override bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction is not EludeCreatureGameAction eludeCreatureGameAction) return false;
            if (!eludeCreatureGameAction.IsSucceed) return false;
            return true;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction is not EludeCreatureGameAction eludeCreatureGameAction) return;
            eludeCreatureGameAction.SuccesEffects.Add(SucessEffect);

            await Task.CompletedTask;

            async Task SucessEffect()
            {
                InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                    .SetWith(canBackToThisInteractable: false, mustShowInCenter: false, "Interactable_Card01581");
                interactableGameAction.CreateContinueMainButton();
                foreach (CardPlace place in eludeCreatureGameAction.ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
                {
                    interactableGameAction.CreateEffect(place, new Stat(0, false), MoveAndUnconfront, PlayActionType.Choose | PlayActionType.Move, playedBy: eludeCreatureGameAction.ActiveInvestigator);

                    async Task MoveAndUnconfront() =>
                        await _gameActionsProvider.Create<MoveInvestigatorAndUnconfrontGameAction>()
                        .SetWith(eludeCreatureGameAction.ActiveInvestigator, place).Execute();
                }

                await interactableGameAction.Execute();
            }
        }
    }
}
