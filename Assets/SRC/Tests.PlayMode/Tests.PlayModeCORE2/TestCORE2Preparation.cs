﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class TestCORE2Preparation : Preparation
    {
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayMode/Tests.PlayModeCORE2/SaveDataCORE2.json";
        protected override string SCENE_NAME => "GamePlayCORE2";
        public SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;
        public override CardPlace StartingPlace => SceneCORE2.Fluvial;

        /*******************************************************************/
        protected override async Task PlaceScene()
        {
            Dictionary<Card, (Zone zone, bool faceDown)> all = GetCardZonesPlacesCORE2().ToDictionary(pair => pair.Key, pair => (pair.Value, false))
                .Concat(GetCardZonesSceneCORE2()).ToDictionary(pair => pair.Key, pair => pair.Value);

            await _gameActionsProvider.Create(new MoveCardsGameAction(all));

            /*******************************************************************/
            Dictionary<Card, Zone> GetCardZonesPlacesCORE2() => new()
            {
                { SceneCORE2.South, SceneCORE2.PlaceZone[0, 3] },
                { SceneCORE2.Hospital, SceneCORE2.PlaceZone[0, 2] },
                { SceneCORE2.Graveyard, SceneCORE2.PlaceZone[1, 4] },
                { SceneCORE2.Fluvial, SceneCORE2.PlaceZone[1, 3] },
                { SceneCORE2.University, SceneCORE2.PlaceZone[1, 2] },
                { SceneCORE2.West, SceneCORE2.PlaceZone[2, 4] },
                { SceneCORE2.Center, SceneCORE2.PlaceZone[2, 3] },
                { SceneCORE2.North, SceneCORE2.PlaceZone[2, 2] },
                { SceneCORE2.Home, SceneCORE2.PlaceZone[0, 4] }
            };

            Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesSceneCORE2()
            {
                Dictionary<Card, (Zone zone, bool faceDown)> moveSceneCards = new()
                    {
                        { SceneCORE2.PlotCards.First(), (SceneCORE2.PlotZone, false )},
                        { SceneCORE2.GoalCards.First(), (SceneCORE2.GoalZone, false)}
                    };

                SceneCORE2.StartDeckDangerCards.ForEach(card => moveSceneCards.Add(card, (SceneCORE2.DangerDeckZone, true)));
                return moveSceneCards;
            }
        }
    }
}