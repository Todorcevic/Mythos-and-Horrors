using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01602 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public bool IsUniqueTarget => true;
        public Investigator TargetInvestigator => Owner;
        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Humanoid, Tag.Cultist, Tag.SilverTwilight };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<CreatureAttackGameAction>(PutEldritchCondition, PutEldritchLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task PutEldritchLogic(CreatureAttackGameAction creatureAttackGameAction)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(_chaptersProvider.CurrentScene.CurrentPlot.Eldritch, 1).Execute();
        }

        private bool PutEldritchCondition(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (creatureAttackGameAction.Creature != this) return false;
            return true;
        }
    }
}
