using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01178 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        public override IEnumerable<Tag> Tags => new[] { Tag.Pact, Tag.Deprivation };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01178"));
            interactableGameAction.CreateCardEffect(this, new Stat(0, false), DrawAndEldritch, PlayActionType.Choose, playedBy: investigator, new Localization("CardEffect_Card01178"));
            interactableGameAction.CreateCardEffect(this, new Stat(0, false), TakeFear, PlayActionType.Choose, playedBy: investigator, new Localization("CardEffect_Card01178-1"));
            await interactableGameAction.Execute();

            /*******************************************************************/
            async Task DrawAndEldritch()
            {
                await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Execute();
                await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Execute();
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch, 2).Execute();
                await _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute();
            }

            async Task TakeFear() => await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountFear: 2).Execute();
        }
    }
}
