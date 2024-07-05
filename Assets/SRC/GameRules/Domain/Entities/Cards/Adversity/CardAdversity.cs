using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversity : Card, IDrawRevelation
    {
        /*******************************************************************/
        public abstract Zone ZoneToMoveWhenDraw(Investigator investigator);

        public abstract Task PlayRevelationFor(Investigator investigator);
    }
}
