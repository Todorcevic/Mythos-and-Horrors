using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DropKeyGameAction : PayKeyGameAction
    {
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statablesUpdated = new()
            {
                { ToStat, ToStat.Value + Amount },
                { Investigator.Hints, Investigator.Hints.Value - Amount}
            };

            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(statablesUpdated).Execute();
        }
    }
}
