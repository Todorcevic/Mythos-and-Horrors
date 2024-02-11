using MythsAndHorrors.GameRules;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{

    public interface IPresenter
    {
        Task CheckGameAction(GameAction gamAction);
    }
}
