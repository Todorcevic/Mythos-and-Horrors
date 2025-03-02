using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class StopUndoGameAction : GameAction
    {
        public override bool CanUndo => false;

        protected override Task ExecuteThisLogic() => Task.CompletedTask;
    }
}

