using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01597 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionRepository;

        public override IEnumerable<Tag> Tags => new[] { Tag.Flaw };
    }
}
