using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CreatureAttackGameAction : GameAction
    {
        public CardCreature Creature { get; private set; }
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Creature.IsInPlay.IsTrue && Investigator.IsInPlay.IsTrue;

        /*******************************************************************/
        public CreatureAttackGameAction SetWith(CardCreature creature, Investigator investigator)
        {
            Creature = creature;
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(Investigator, Creature, Creature.Damage.Value, Creature.Fear.Value).Execute();
        }
    }
}