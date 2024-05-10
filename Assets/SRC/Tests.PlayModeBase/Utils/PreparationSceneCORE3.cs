using MythosAndHorrors.GameRules;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PreparationSceneCORE3 : Preparation
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        public SceneCORE3 SceneCORE3 => (SceneCORE3)_chaptersProvider.CurrentScene;
        protected override CardPlace StartingPlace => throw new System.NotImplementedException();

        /*******************************************************************/
        public override IEnumerator PlaceAllScene()
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            Dictionary<Card, (Zone zone, bool faceDown)> all = GetCardZonesPlacesCORE3().ToDictionary(pair => pair.Key, pair => (pair.Value, false))
                .Concat(GetCardZonesSceneCORE3()).ToDictionary(pair => pair.Key, pair => pair.Value);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(all)).AsCoroutine();
            Time.timeScale = currentTimeScale;

            /*******************************************************************/
            Dictionary<Card, Zone> GetCardZonesPlacesCORE3() => new()
            {
                { SceneCORE3.MainPath, SceneCORE3.PlaceZone[1, 3] },
                { SceneCORE3.Forests[0], SceneCORE3.PlaceZone[0, 2] },
                { SceneCORE3.Forests[1], SceneCORE3.PlaceZone[0, 4] },
                { SceneCORE3.Forests[2], SceneCORE3.PlaceZone[2, 2] },
                { SceneCORE3.Forests[3], SceneCORE3.PlaceZone[2, 4] },
            };

            Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesSceneCORE3()
            {
                Dictionary<Card, (Zone zone, bool faceDown)> moveSceneCards = new()
                    {
                        { SceneCORE3.Info.PlotCards.First(), (SceneCORE3.PlotZone, false )},
                        { SceneCORE3.Info.GoalCards.First(), (SceneCORE3.GoalZone, false)}
                    };

                SceneCORE3.RealDangerCards.ForEach(card => moveSceneCards.Add(card, (SceneCORE3.DangerDeckZone, true)));
                return moveSceneCards;
            }
        }
    }
}
