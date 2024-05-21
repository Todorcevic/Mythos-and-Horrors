using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01182 : CardAdversity
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;


        public override IEnumerable<Tag> Tags => new[] { Tag.Omen };


    }
}
