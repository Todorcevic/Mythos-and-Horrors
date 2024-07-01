using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class AddSlotGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Slot Slot { get; private set; }

        /*******************************************************************/
        public AddSlotGameAction SetWith(Investigator cardSupply, Slot slot)
        {
            Investigator = cardSupply;
            Slot = slot;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.SlotsCollection.AddSlot(Slot);
            await Task.CompletedTask;
        }

        public override async Task Undo()
        {
            Investigator.SlotsCollection.RemoveSlot(Slot);
            await Task.CompletedTask;
        }
    }
}
