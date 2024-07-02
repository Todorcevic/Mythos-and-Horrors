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

        private CardPlace Study => _cardsProvider.GetCard<Card01111>();
        private CardPlace Hallway => _cardsProvider.GetCard<Card01112>();
        private CardPlace Attic => _cardsProvider.GetCard<Card01113>();
        private CardPlace Cellar => _cardsProvider.GetCard<Card01114>();
        private CardPlace Parlor => _cardsProvider.GetCard<Card01115>();
        private IEnumerable<CardCreature> CreaturesInStudy() => _cardsProvider.AllCards.OfType<CardCreature>()
          .Where(cardCreature => cardCreature.CurrentPlace == Study);

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            Dictionary<Card, Zone> allPlaces = new()
            {
                { Hallway, _chaptersProvider.CurrentScene.GetPlaceZone(1, 3) },
                { Cellar, _chaptersProvider.CurrentScene.GetPlaceZone(0, 4) },
                { Attic, _chaptersProvider.CurrentScene.GetPlaceZone(2, 4) },
                { Parlor, _chaptersProvider.CurrentScene.GetPlaceZone(1, 4) }
            };

            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(allPlaces).Execute();
            await _gameActionsProvider.Create<SafeForeach<CardCreature>>().SetWith(CreaturesInStudy, DiscardCreature).Execute();
            await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay, Hallway).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(Study, _chaptersProvider.CurrentScene.OutZone).Execute();
        }

        private async Task DiscardCreature(CardCreature creature) => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(creature).Execute();
    }
}
