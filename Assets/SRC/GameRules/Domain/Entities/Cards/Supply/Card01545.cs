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
            CreateActivation(1, InvestigationLogic, InvestigationCondition, PlayActionType.Investigate);
        }

        /*******************************************************************/
        private async Task InvestigationLogic(Investigator investigator)
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

        private bool InvestigationCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != investigator) return false;
            return true;
        }
    }
}
