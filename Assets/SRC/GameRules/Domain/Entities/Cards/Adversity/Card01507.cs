using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01507 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Task };

        public override Zone ZoneToMove => _chaptersProvider.CurrentScene.LimboZone;


    }
}
