using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureAttackGameAction : GameAction
    {
        [Inject] private readonly IPresenter<CreatureAttackGameAction> _creatureAttackPresenter;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public CardCreature Creature { get; }
        public Investigator Investigator { get; }

        /*******************************************************************/
        public CreatureAttackGameAction(CardCreature creature, Investigator investigator)
        {
            Creature = creature;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _creatureAttackPresenter.PlayAnimationWith(this);

            Dictionary<Stat, int> statsWithValues = new()
            {
                {Investigator.Health, Creature.Damage.Value},
                {Investigator.Sanity, Creature.Fear.Value}
            };
            await _gameActionFactory.Create(new DecrementStatGameAction(statsWithValues));
        }
    }
}