using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01165 : CardAdversity
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;


        public override IEnumerable<Tag> Tags => new[] { Tag.Terror };

    }
}
