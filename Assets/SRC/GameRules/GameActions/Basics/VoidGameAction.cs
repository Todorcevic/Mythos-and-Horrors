using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class VoidGameAction : GameAction
    {
        protected override Task ExecuteThisLogic() => Task.CompletedTask;
    }
}

