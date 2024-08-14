using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureAttackGameAction : GameAction
    {
        [Inject] private readonly IPresenter<CreatureAttackGameAction> _creatureAttackPresenter;

        public CardCreature Creature { get; private set; }
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Creature.IsInPlay && Investigator.IsInPlay;

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
            await _creatureAttackPresenter.PlayAnimationWith(this);
            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(Investigator, Creature, Creature.Damage.Value, Creature.Fear.Value).Execute();
        }
    }
}