using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class PositionTest : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator All_Zones_With_Cards()
        {
            Investigator investigator1 = _investigatorsProvider.First;
            foreach (Slot slot in investigator1.SlotsCollection.Slots.ToList())
            {
                yield return _gameActionsProvider.Create<AddSlotGameAction>().SetWith(investigator1, slot).Start().AsCoroutine();
                yield return _gameActionsProvider.Create<AddSlotGameAction>().SetWith(investigator1, slot).Start().AsCoroutine();
                yield return _gameActionsProvider.Create<AddSlotGameAction>().SetWith(investigator1, slot).Start().AsCoroutine();
                yield return _gameActionsProvider.Create<AddSlotGameAction>().SetWith(investigator1, slot).Start().AsCoroutine();
            }

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
                    {_chaptersProvider.CurrentScene.Cards[30], _chaptersProvider.CurrentScene.PlotZone},
                    {_chaptersProvider.CurrentScene.Cards[31], _chaptersProvider.CurrentScene.GoalZone},
                    {_chaptersProvider.CurrentScene.Cards[3], _chaptersProvider.CurrentScene.DangerDeckZone},
                    {_chaptersProvider.CurrentScene.Cards[26], _chaptersProvider.CurrentScene.DangerDiscardZone},
                    {_chaptersProvider.CurrentScene.Cards[32], _chaptersProvider.CurrentScene.LimboZone},
            };

            int k = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    cardsWithZone.Add(_chaptersProvider.CurrentScene.Cards[5 + k++], _chaptersProvider.CurrentScene.GetPlaceZone(i, j));
                }
            }

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardsWithZone).Start().AsCoroutine();


            if (DEBUG_MODE) yield return PressAnyKey();

            Assert.That(true);
        }
    }
}
