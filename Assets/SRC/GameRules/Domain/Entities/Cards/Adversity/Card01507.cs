using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01507 : CardAdversity, IFlaw
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

    }
}
