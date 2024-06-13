using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayRevelationAdversityGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardAdversity CardAdversity { get; }
        public Investigator Investigator { get; }

        /*******************************************************************/
        public PlayRevelationAdversityGameAction(CardAdversity cardAdversity, Investigator investigator)
        {
            CardAdversity = cardAdversity;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(CardAdversity, CardAdversity.ZoneToMoveWhenDraw(Investigator)));
            await CardAdversity.PlayAdversityFor(Investigator);
        }
    }
}
