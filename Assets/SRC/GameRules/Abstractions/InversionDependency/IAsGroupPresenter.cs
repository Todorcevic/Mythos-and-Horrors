using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IAsGroupPresenter
    {
        Task<Dictionary<Card, int>> SelectWith(GameAction gamAction);
    }
}
