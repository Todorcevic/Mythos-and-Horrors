using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversity : Card, IDrawActivable
    {
        /*******************************************************************/
        public abstract Zone ZoneToMoveWhenDraw(Investigator investigator);

        public abstract Task PlayRevelationFor(Investigator investigator);
    }
}
