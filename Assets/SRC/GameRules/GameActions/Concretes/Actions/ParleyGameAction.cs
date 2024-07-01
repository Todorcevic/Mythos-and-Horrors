using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ParleyGameAction : GameAction
    {
        public Func<Task> ParleyLogic { get; private set; }

        /*******************************************************************/
        public ParleyGameAction SetWith(Func<Task> parleyLogic)
        {
            ParleyLogic = parleyLogic;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await ParleyLogic.Invoke();
        }
    }
}
