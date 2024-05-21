using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01168 : CardAdversity
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;


        public override IEnumerable<Tag> Tags => new[] { Tag.Hazard };
    }
}
