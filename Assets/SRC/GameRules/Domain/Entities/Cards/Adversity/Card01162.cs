using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01162 : CardAdversity
    {

        [Inject] private readonly ChaptersProvider _chaptersProvider; 
        public override Zone ZoneToMove => _chaptersProvider.CurrentScene.LimboZone;

    }
}
