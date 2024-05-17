using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestCORE1Preparation : Preparation
    {
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.Base.New/Tests.PlayModeCORE1/SaveDataCORE1.json";
        public SceneCORE1 SceneCORE1 => (SceneCORE1)_chaptersProvider.CurrentScene;
        public override CardPlace StartingPlace => SceneCORE1.Study;

        /*******************************************************************/
        protected override async Task PlaceScene()
        {
            Dictionary<Card, (Zone zone, bool faceDown)> all = GetCardZonesPlacesCORE1().ToDictionary(pair => pair.Key, pair => (pair.Value, false))
                .Concat(GetCardZonesSceneCORE1()).ToDictionary(pair => pair.Key, pair => pair.Value);

            await _gameActionsProvider.Create(new MoveCardsGameAction(all));

            /*******************************************************************/
            Dictionary<Card, Zone> GetCardZonesPlacesCORE1() => new()
            {
                    { SceneCORE1.Study, SceneCORE1.PlaceZone[0, 3] },
                    { SceneCORE1.Hallway, SceneCORE1.PlaceZone[1, 3] },
                    { SceneCORE1.Attic, SceneCORE1.PlaceZone[2, 4] },
                    { SceneCORE1.Cellar, SceneCORE1.PlaceZone[0, 4] },
                    { SceneCORE1.Parlor, SceneCORE1.PlaceZone[1, 4] }
            };

            Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesSceneCORE1()
            {
                Dictionary<Card, (Zone zone, bool faceDown)> moveSceneCards = new()
                    {
                        { SceneCORE1.Info.PlotCards.First(), (SceneCORE1.PlotZone, false )},
                        { SceneCORE1.Info.GoalCards.First(), (SceneCORE1.GoalZone, false)}
                    };

                SceneCORE1.StartDangerCards.ForEach(card => moveSceneCards.Add(card, (SceneCORE1.DangerDeckZone, true)));
                return moveSceneCards;
            }
        }
    }
}