using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01597 : CardAdversity, IFlaw
    {
        [Inject] private readonly GameActionsProvider _gameActionRepository;


    }
}
