using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class AddRequerimentCardGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Card Card { get; private set; }

        /*******************************************************************/
        public AddRequerimentCardGameAction SetWith(Investigator investigator, Card card)
        {
            Investigator = investigator;
            Card = card;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.Cards.Add(Card);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            Investigator.Cards.Remove(Card);
            await base.Undo();
        }
    }
}

