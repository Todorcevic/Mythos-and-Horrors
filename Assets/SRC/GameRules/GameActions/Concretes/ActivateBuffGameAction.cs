using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class ActivateBuffGameAction : GameAction
    {
        public Buff Buff { get; }

        /*******************************************************************/
        public ActivateBuffGameAction(Buff buff)
        {
            Buff = buff;
        }
        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await Buff.Apply();
        }
    }
}
