using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class PositionTest : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator All_Zones_With_Cards()
        {
            Investigator investigator1 = _investigatorsProvider.First;

            Dictionary<Card, Zone> cardsWithZone = new()
            {
                    {investigator1.InvestigatorCard, investigator1.InvestigatorZone},
                    {investigator1.Cards[1], investigator1.DiscardZone},
                    {investigator1.Cards[2], investigator1.DeckZone},
                    {investigator1.Cards[3], investigator1.AidZone},
                    {investigator1.Cards[4], investigator1.AidZone},
                    {investigator1.Cards[5], investigator1.AidZone},
                    {investigator1.Cards[6], investigator1.AidZone},
                    {investigator1.Cards[7], investigator1.AidZone},
                    {investigator1.Cards[8], investigator1.DangerZone},
                    {investigator1.Cards[9], investigator1.DangerZone},
                    {investigator1.Cards[10], investigator1.DangerZone},
                    {investigator1.Cards[11], investigator1.DangerZone},
                    {investigator1.Cards[12], investigator1.DangerZone},
                    {investigator1.Cards[13], investigator1.HandZone},
                    {investigator1.Cards[14], investigator1.HandZone},
                    {investigator1.Cards[15], investigator1.HandZone},
                    {investigator1.Cards[16], investigator1.HandZone},
                    {investigator1.Cards[17], investigator1.HandZone},
                    {investigator1.Cards[18], investigator1.HandZone},
                    {investigator1.Cards[19], investigator1.HandZone},
                    {investigator1.Cards[20], investigator1.HandZone},
                    {investigator1.Cards[21], investigator1.DiscardZone},
                    {investigator1.Cards[22], investigator1.DiscardZone},
                    {investigator1.Cards[23], investigator1.DiscardZone},
                    {investigator1.Cards[24], investigator1.DiscardZone},
                    {_chaptersProvider.CurrentScene.Info.Cards[30], _chaptersProvider.CurrentScene.PlotZone},
                    {_chaptersProvider.CurrentScene.Info.Cards[31], _chaptersProvider.CurrentScene.GoalZone},
                    {_chaptersProvider.CurrentScene.Info.Cards[3], _chaptersProvider.CurrentScene.DangerDeckZone},
                    {_chaptersProvider.CurrentScene.Info.Cards[26], _chaptersProvider.CurrentScene.DangerDiscardZone},
                    {_chaptersProvider.CurrentScene.Info.Cards[32], _chaptersProvider.CurrentScene.LimboZone},
            };



            int k = 0;
            for (int i = 0; i < _chaptersProvider.CurrentScene.PlaceZone.GetLength(0); i++)
            {
                for (int j = 0; j < _chaptersProvider.CurrentScene.PlaceZone.GetLength(1); j++)
                {
                    cardsWithZone.Add(_chaptersProvider.CurrentScene.Info.Cards[5 + k++], _chaptersProvider.CurrentScene.PlaceZone[i, j]);
                }
            }

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardsWithZone)).AsCoroutine();


            if (DEBUG_MODE) yield return PressAnyKey();

            Assert.That(true);
        }
    }
}
