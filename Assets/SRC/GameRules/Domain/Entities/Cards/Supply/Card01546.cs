using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01546 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent, Tag.Illicit };

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            CreateOptativeReaction<EludeGameAction>(DrawLogic, DrawCondition, GameActionTime.After, "OptativeReaction_Card01546");
        }

        /*******************************************************************/
        private async Task DrawCondition(EludeGameAction eludeGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(ControlOwner).Execute();
        }

        private bool DrawLogic(EludeGameAction eludeGameAction)
        {
            if (!IsInPlay.IsTrue) return false;
            if (eludeGameAction.Investigator != ControlOwner) return false;
            if (Exausted.IsActive) return false;
            return true;
        }
    }
}
