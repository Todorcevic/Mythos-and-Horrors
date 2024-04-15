using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01108 : CardGoal
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        CardPlace Study => _cardsProvider.GetCard<Card01111>();
        CardPlace Hallway => _cardsProvider.GetCard<Card01112>();
        CardPlace Attic => _cardsProvider.GetCard<Card01113>();
        CardPlace Cellar => _cardsProvider.GetCard<Card01114>();
        CardPlace Parlor => _cardsProvider.GetCard<Card01115>();

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            Dictionary<Card, Zone> allPlaces = new()
            {
                { Hallway, _chaptersProvider.CurrentScene.PlaceZone[1, 3] },
                { Cellar, _chaptersProvider.CurrentScene.PlaceZone[0, 4] },
                { Attic, _chaptersProvider.CurrentScene.PlaceZone[2, 4] },
                { Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 4] }
            };

            await _gameActionsProvider.Create(new MoveCardsGameAction(allPlaces));
            await new SafeForeach<CardCreature>(DiscardCreature, GetCreatures).Execute();
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Hallway));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Study, _chaptersProvider.CurrentScene.OutZone));
        }

        private IEnumerable<CardCreature> GetCreatures() => _cardsProvider.AllCards.OfType<CardCreature>()
            .Where(cardCreature => cardCreature.CurrentPlace == Study);

        private async Task DiscardCreature(CardCreature creature) => await _gameActionsProvider.Create(new DiscardGameAction(creature));
    }
}
