using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01545 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent, Tag.Illicit };

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            CreateFastActivation(InvestigationLogic, InvestigationCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private async Task InvestigationLogic(Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
               .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, code: "Card01545");
            interactable.CreateEffect(investigator.CurrentPlace, investigator.CurrentPlace.InvestigationTurnsCost, Investigate, PlayActionType.Investigate, investigator, cardAffected: this);
            await interactable.Execute();

            /*******************************************************************/
            async Task Investigate()
            {
                await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
                InvestigatePlaceGameAction investigate = _gameActionsProvider.Create<InvestigatePlaceGameAction>()
                    .SetWith(investigator, investigator.CurrentPlace);
                investigate.SuccesEffects.Clear();
                investigate.SuccesEffects.Add(GainResources);
                await investigate.Execute();

                /*******************************************************************/
                async Task GainResources() => await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, 3).Execute();
            }
        }

        private bool InvestigationCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != investigator) return false;
            if (!investigator.CanInvestigate) return false;
            if (!ControlOwner.CurrentPlace.CanBeInvestigated.IsActive) return false;
            return true;
        }
    }
}
