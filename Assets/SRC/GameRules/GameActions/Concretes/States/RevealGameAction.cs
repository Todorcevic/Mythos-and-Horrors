using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class RevealGameAction : GameAction
    {
        public IRevealable RevellableCard { get; private set; }
        public Card Card => RevellableCard as Card;

        /*******************************************************************/
        public RevealGameAction SetWith(IRevealable cardReveled)
        {
            RevellableCard = cardReveled;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(RevellableCard.Revealed, true).Start();
            await RevellableCard.RevealCommand.RunWith(this);
        }
    }
}
