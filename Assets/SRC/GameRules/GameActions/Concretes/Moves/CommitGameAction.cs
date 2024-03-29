using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    internal class CommitGameAction : GameAction
    {
        private Card _commitableCard;

        /*******************************************************************/
        public CommitGameAction(Card card)
        {
            _commitableCard = card;
        }

        /*******************************************************************/
        protected override Task ExecuteThisLogic()
        {
            throw new System.NotImplementedException();
        }
    }
}