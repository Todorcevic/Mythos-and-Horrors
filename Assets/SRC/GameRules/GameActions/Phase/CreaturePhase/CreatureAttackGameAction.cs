using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureAttackGameAction : GameAction
    {
        [Inject] private readonly IPresenter<CreatureAttackGameAction> _creatureAttackPresenter;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public CardCreature Creature { get; private set; }

        /*******************************************************************/
        public CreatureAttackGameAction(CardCreature creature)
        {
            Creature = creature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _creatureAttackPresenter.PlayAnimationWith(this);
            await _gameActionFactory.Create(new DecrementStatGameAction(Creature.ConfrontedInvestigator.Health, Creature.Damage.Value));
            await _gameActionFactory.Create(new DecrementStatGameAction(Creature.ConfrontedInvestigator.Sanity, Creature.Fear.Value));
        }
    }
}