using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class TestCORE1Preparation : Preparation
    {
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayMode/Tests.PlayModeCORE1/SaveDataCORE1.json";
        protected override string SCENE_NAME => "GamePlayCORE1";
        public SceneCORE1 SceneCORE1 => (SceneCORE1)_chaptersProvider.CurrentScene;
        public override CardPlace StartingPlace => SceneCORE1.Study;

        /*******************************************************************/
        protected override async Task PlaceScene()
        {
            Dictionary<Card, (Zone zone, bool faceDown)> all = GetCardZonesPlacesCORE1().ToDictionary(pair => pair.Key, pair => (pair.Value, false))
                .Concat(GetCardZonesSceneCORE1()).ToDictionary(pair => pair.Key, pair => pair.Value);

            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(all).Start();

            /*******************************************************************/
            Dictionary<Card, Zone> GetCardZonesPlacesCORE1() => new()
            {
                    { SceneCORE1.Study, SceneCORE1.GetPlaceZone(0, 3) },
                    { SceneCORE1.Hallway, SceneCORE1.GetPlaceZone(1, 3) },
                    { SceneCORE1.Attic, SceneCORE1.GetPlaceZone(2, 4) },
                    { SceneCORE1.Cellar, SceneCORE1.GetPlaceZone(0, 4) },
                    { SceneCORE1.Parlor, SceneCORE1.GetPlaceZone(1, 4) }
            };

            Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesSceneCORE1()
            {
                Dictionary<Card, (Zone zone, bool faceDown)> moveSceneCards = new()
                    {
                        { SceneCORE1.PlotCards.First(), (SceneCORE1.PlotZone, false )},
                        { SceneCORE1.GoalCards.First(), (SceneCORE1.GoalZone, false)}
                    };

                SceneCORE1.StartDeckDangerCards.ForEach(card => moveSceneCards.Add(card, (SceneCORE1.DangerDeckZone, true)));
                return moveSceneCards;
            }
        }
    }
}