using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IDrawActivable
    {
        Zone ZoneToMoveWhenDraw(Investigator investigator);

        Task PlayRevelationFor(Investigator investigator);
    }
}
