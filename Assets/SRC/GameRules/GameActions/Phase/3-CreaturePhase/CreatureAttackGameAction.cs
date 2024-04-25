using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureAttackGameAction : GameAction
    {
        [Inject] private readonly IPresenter<CreatureAttackGameAction> _creatureAttackPresenter;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

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
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(Investigator, Creature, Creature.Damage.Value, Creature.Fear.Value));
        }
    }
}