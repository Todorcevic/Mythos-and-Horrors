using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IDrawRevelation
    {
        Zone ZoneToMoveWhenDraw(Investigator investigator);

        Task PlayRevelationFor(Investigator investigator);
    }
}
