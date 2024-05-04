using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01118 : CardCreature, ISpawnable
    {
        [Inject] private readonly CardsProvider _cardsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Ghoul };
        public CardPlace SpawnPlace => _cardsProvider.GetCard<Card01114>();

    }
}
