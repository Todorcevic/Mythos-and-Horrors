using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01174 : CardAdversity
    {
        public override Zone ZoneToMove => Owner.DangerZone;

    }
}
