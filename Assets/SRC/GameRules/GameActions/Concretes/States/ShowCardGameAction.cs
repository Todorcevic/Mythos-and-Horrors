using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ShowCardGameAction : GameAction
    {
        public Card Card { get; private set; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public ShowCardGameAction SetWith(Card card)
        {
            Card = card;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Card.FaceDown, false).Execute();
        }
    }
}
