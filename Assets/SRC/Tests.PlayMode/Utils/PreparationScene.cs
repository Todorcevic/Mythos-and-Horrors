using MythosAndHorrors.GameRules;
using Zenject;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MythosAndHorrors.GameView;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PreparationScene
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public SceneCORE1 SceneCORE1 => (SceneCORE1)_chaptersProvider.CurrentScene;

        /*******************************************************************/
        private Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesInvestigator(Investigator investigator)
        {
            Dictionary<Card, (Zone zone, bool faceDown)> moveInvestigatorCards = new()
            {
                { investigator.InvestigatorCard, (investigator.InvestigatorZone, false) }
            };

            investigator.FullDeck.Take(5).ForEach(card => moveInvestigatorCards.Add(card, (investigator.HandZone, false)));
            investigator.FullDeck.Skip(5).ForEach(card => moveInvestigatorCards.Add(card, (investigator.DeckZone, true)));
            return moveInvestigatorCards;
        }

        private Dictionary<Card, Zone> GetCardZonesPlaces()
        {
            return new()
            {
                { SceneCORE1.Study, _chaptersProvider.CurrentScene.PlaceZone[0, 3] },
                { SceneCORE1.Hallway, _chaptersProvider.CurrentScene.PlaceZone[1, 3] },
                { SceneCORE1.Attic, _chaptersProvider.CurrentScene.PlaceZone[2, 4] },
                { SceneCORE1.Cellar, _chaptersProvider.CurrentScene.PlaceZone[0, 4] },
                { SceneCORE1.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 4] }
            };
        }

        private Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesScene()
        {
            Dictionary<Card, (Zone zone, bool faceDown)> moveSceneCards = new()
            {
                { SceneCORE1.Info.PlotCards.First(), (SceneCORE1.PlotZone, false )},
                { SceneCORE1.Info.GoalCards.First(), (SceneCORE1.GoalZone, false)}
            };
            SceneCORE1.RealDangerCards.ForEach(card => moveSceneCards.Add(card, (SceneCORE1.DangerDeckZone, true)));

            return moveSceneCards;
        }

        public IEnumerator PlaceAllPlaceCards()
        {
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(GetCardZonesPlaces()));
        }

        public IEnumerator PlayThisInvestigator(Investigator investigator)
        {
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(GetCardZonesInvestigator(investigator)));
            yield return _gameActionsProvider.Create(new GainResourceGameAction(investigator, 5));
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Study));

            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

        public IEnumerator PlayAllInvestigators()
        {
            Dictionary<Card, (Zone zone, bool faceDown)> toMoveAllInvestigators = new();
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                yield return PlayThisInvestigator(investigator);
            }
        }

        public IEnumerator PlaceAllSceneCards()
        {
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(GetCardZonesScene()));
        }

        public IEnumerator StartingScene()
        {
            yield return PlaceAllSceneCards();
            yield return PlaceAllPlaceCards();
            yield return PlayAllInvestigators();
        }
    }
}
