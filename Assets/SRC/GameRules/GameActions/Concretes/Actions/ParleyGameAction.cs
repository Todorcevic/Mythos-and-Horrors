using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{

    public class ParleyGameAction : GameAction
    {
        public Func<Task> ParleyLogic { get; }

        /*******************************************************************/
        public ParleyGameAction(Func<Task> parleyLogic)
        {
            ParleyLogic = parleyLogic;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await ParleyLogic.Invoke();
        }
    }
}
