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
            CreateActivation(1, InvestigationLogic, InvestigationCondition, PlayActionType.Activate | PlayActionType.Investigate, new Localization("Activation_Card01545"));
        }

        /*******************************************************************/
        private async Task InvestigationLogic(Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
               .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01545"));
            interactable.CreateCardEffect(investigator.CurrentPlace, new Stat(0, false), Investigate,
                PlayActionType.Choose, investigator, new Localization("CardEffect_Card01545"), cardAffected: this);
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
            if (!IsInPlay.IsTrue) return false;
            if (ControlOwner != investigator) return false;
            if (!investigator.CurrentPlace.CanBeInvestigated.IsTrue) return false;
            return true;
        }
    }
}
