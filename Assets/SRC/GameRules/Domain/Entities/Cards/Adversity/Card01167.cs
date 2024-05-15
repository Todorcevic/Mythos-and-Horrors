using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01167 : CardAdversity
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override Zone ZoneToMove => _chaptersProvider.CurrentScene.LimboZone;

        public override IEnumerable<Tag> Tags => new[] { Tag.Hazard };

    }
}
