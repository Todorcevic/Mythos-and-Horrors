using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public class PreparationSceneCORE2 : Preparation
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        public SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;

        public override CardPlace StartingPlace => SceneCORE2.Fluvial;

        /*******************************************************************/
        public override async Task PlaceAllScene()
        {
            Dictionary<Card, (Zone zone, bool faceDown)> all = GetCardZonesPlacesCORE2().ToDictionary(pair => pair.Key, pair => (pair.Value, false))
                .Concat(GetCardZonesSceneCORE2()).ToDictionary(pair => pair.Key, pair => pair.Value);

            await _gameActionsProvider.Create(new MoveCardsGameAction(all));

            /*******************************************************************/
            Dictionary<Card, Zone> GetCardZonesPlacesCORE2() => new()
            {
                { _cardsProvider.GetCard<Card01126>(), SceneCORE2.PlaceZone[0, 3] },
                { SceneCORE2.Hospital, SceneCORE2.PlaceZone[0, 2] },
                { SceneCORE2.Graveyard, SceneCORE2.PlaceZone[1, 4] },
                { SceneCORE2.Fluvial, SceneCORE2.PlaceZone[1, 3] },
                { SceneCORE2.University, SceneCORE2.PlaceZone[1, 2] },
                { SceneCORE2.West, SceneCORE2.PlaceZone[2, 4] },
                { _cardsProvider.GetCard<Card01130>(), SceneCORE2.PlaceZone[2, 3] },
                { SceneCORE2.North, SceneCORE2.PlaceZone[2, 2] },
                { SceneCORE2.Home, SceneCORE2.PlaceZone[0, 4] }
            };

            Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesSceneCORE2()
            {
                Dictionary<Card, (Zone zone, bool faceDown)> moveSceneCards = new()
                    {
                        { SceneCORE2.Info.PlotCards.First(), (SceneCORE2.PlotZone, false )},
                        { SceneCORE2.Info.GoalCards.First(), (SceneCORE2.GoalZone, false)}
                    };

                SceneCORE2.StartDangerCards.ForEach(card => moveSceneCards.Add(card, (SceneCORE2.DangerDeckZone, true)));
                return moveSceneCards;
            }
        }
    }
}
