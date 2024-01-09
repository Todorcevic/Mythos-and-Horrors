using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        public List<Card> ActivableCards { get; protected set; }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await Task.CompletedTask;
        }
    }
}

