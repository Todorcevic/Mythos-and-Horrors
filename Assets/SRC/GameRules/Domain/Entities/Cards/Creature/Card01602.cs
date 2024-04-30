using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01602 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public bool IsOnlyOneTarget => true;
        public Investigator TargetInvestigator => Owner;

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await Reaction<CreatureAttackGameAction>(gameAction, PutEldritchCondition, PutEldritchLogic); //TODO test
        }

        /*******************************************************************/
        private async Task PutEldritchLogic(CreatureAttackGameAction creatureAttackGameAction)
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(_chaptersProvider.CurrentScene.CurrentPlot.Eldritch, 1));
        }

        private bool PutEldritchCondition(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (creatureAttackGameAction.Creature != this) return false;
            return true;
        }
    }
}
