using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01168 : CardAdversity
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override Zone ZoneToMove => _chaptersProvider.CurrentScene.LimboZone;
    }
}
