using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class EludeGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardCreature CardCreature { get; }

        /*******************************************************************/
        public EludeGameAction(CardCreature creature)
        {
            CardCreature = creature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(CardCreature.Exausted, true));
            await _gameActionsProvider.Create(new MoveCardsGameAction(CardCreature, CardCreature.CurrentPlace.OwnZone));
        }
    }
}
