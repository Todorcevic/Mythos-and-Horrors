using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class TestCORE3Preparation : Preparation
    {
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayMode/Tests.PlayModeCORE3/SaveDataCORE3.json";
        protected override string SCENE_NAME => "GamePlayCORE1";
        public SceneCORE3 SceneCORE3 => (SceneCORE3)_chaptersProvider.CurrentScene;
        public override CardPlace StartingPlace => SceneCORE3.MainPath;

        /*******************************************************************/
        protected override async Task PlaceScene()
        {
            Dictionary<Card, (Zone zone, bool faceDown)> all = GetCardZonesPlacesCORE3().ToDictionary(pair => pair.Key, pair => (pair.Value, false))
                .Concat(GetCardZonesSceneCORE3()).ToDictionary(pair => pair.Key, pair => pair.Value);

            await _gameActionsProvider.Create(new MoveCardsGameAction(all));

            /*******************************************************************/
            Dictionary<Card, Zone> GetCardZonesPlacesCORE3() => new()
            {
                { SceneCORE3.MainPath, SceneCORE3.GetPlaceZone(1, 3) },
                { SceneCORE3.Forests[0], SceneCORE3.GetPlaceZone(0, 2) },
                { SceneCORE3.Forests[1], SceneCORE3.GetPlaceZone(0, 4) },
                { SceneCORE3.Forests[2], SceneCORE3.GetPlaceZone(2, 2) },
                { SceneCORE3.Forests[3], SceneCORE3.GetPlaceZone(2, 4) },
            };

            Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesSceneCORE3()
            {
                Dictionary<Card, (Zone zone, bool faceDown)> moveSceneCards = new()
                    {
                        { SceneCORE3.PlotCards.First(), (SceneCORE3.PlotZone, false )},
                        { SceneCORE3.GoalCards.First(), (SceneCORE3.GoalZone, false)}
                    };

                SceneCORE3.StartDeckDangerCards.ForEach(card => moveSceneCards.Add(card, (SceneCORE3.DangerDeckZone, true)));
                return moveSceneCards;
            }
        }
    }
}