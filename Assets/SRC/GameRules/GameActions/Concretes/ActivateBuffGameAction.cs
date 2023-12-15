using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class ActivateBuffGameAction : GameAction
    {
        private Buff _buff;

        /*******************************************************************/
        public async Task Run(Buff buff)
        {
            _buff = buff;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _buff.Apply();
        }
    }
}
