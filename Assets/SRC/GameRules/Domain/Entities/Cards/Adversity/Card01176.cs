using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01176 : CardAdversity
    {
        public override Zone ZoneToMove => Owner.DangerZone;

        protected override async Task ObligationLogic()
        {
            await Task.CompletedTask;
        }
    }
}
