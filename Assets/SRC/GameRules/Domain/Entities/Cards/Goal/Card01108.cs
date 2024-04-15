using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01108 : CardGoal
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            CardPlace hallway = _cardsProvider.GetCard<Card01112>();
            CardPlace attic = _cardsProvider.GetCard<Card01113>();
            CardPlace cellar = _cardsProvider.GetCard<Card01114>();
            CardPlace parlor = _cardsProvider.GetCard<Card01115>();
            await _gameActionsProvider.Create(new MoveCardsGameAction(hallway, _chaptersProvider.CurrentScene.PlaceZone[1, 3]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(cellar, _chaptersProvider.CurrentScene.PlaceZone[0, 4]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(attic, _chaptersProvider.CurrentScene.PlaceZone[2, 4]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 4]));
        }
    }
}
