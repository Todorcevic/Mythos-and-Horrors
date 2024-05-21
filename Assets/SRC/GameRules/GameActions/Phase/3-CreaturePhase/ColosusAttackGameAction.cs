using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ColosusAttackGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardColosus Colosus { get; }

        /*******************************************************************/
        public ColosusAttackGameAction(CardColosus colosus)
        {
            Colosus = colosus;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(() => Colosus.MassiveInvestigatorsConfronted, Attack));

            async Task Attack(Investigator investigator)
            {
                await _gameActionsProvider.Create(new CreatureAttackGameAction(Colosus, investigator));
            }
        }
    }
}