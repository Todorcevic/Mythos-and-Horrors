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
            InvestigatePlaceGameAction investigate = new(investigator, investigator.CurrentPlace);
            investigate.SuccesEffects.Clear();
            investigate.SuccesEffects.Add(GainResources);

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Start();
            await _gameActionsProvider.Create(investigate);

            /*******************************************************************/
            async Task GainResources() => await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 3));
        }

        private bool InvestigationCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != investigator) return false;
            return true;
        }
    }
}
