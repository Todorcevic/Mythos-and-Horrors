using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01178 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        public override IEnumerable<Tag> Tags => new[] { Tag.Pact, Tag.Isolate };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>().SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Choose");
            interactableGameAction.CreateEffect(this, new Stat(0, false), DrawAndEldritch, PlayActionType.Choose, playedBy: investigator);
            interactableGameAction.CreateEffect(this, new Stat(0, false), TakeFear, PlayActionType.Choose, playedBy: investigator);
            await interactableGameAction.Start();

            async Task DrawAndEldritch()
            {
                await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Start();
                await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Start();
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch, 2).Start();
                await _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            }

            async Task TakeFear()
            {
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountFear: 2).Start();
            }
        }
    }
}
