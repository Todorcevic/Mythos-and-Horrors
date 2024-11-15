﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardCreature01157Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator AttackToAll()
        {
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE3.Khargath, SceneCORE3.MainPath).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<CreatureConfrontAttackGameAction>().Execute().AsCoroutine();

            Assert.That(_investigatorsProvider.First.DamageRecived.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.DamageRecived.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Third.DamageRecived.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.First.FearRecived.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.FearRecived.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Third.FearRecived.Value, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator OpportunityAttack()
        {
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE3.Khargath, SceneCORE3.MainPath).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Second).Execute();
            yield return ClickedIn(SceneCORE3.Forest3);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.DamageRecived.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Third.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.First.FearRecived.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.FearRecived.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Third.FearRecived.Value, Is.EqualTo(0));
            Assert.That(SceneCORE3.Khargath.CurrentPlace, Is.EqualTo(SceneCORE3.MainPath));
        }

        [UnityTest]
        public IEnumerator ThrowLita()
        {
            yield return BuildCard("01117", _investigatorsProvider.Second);
            SceneCORE3.ExecutePrivateMethod("ThrowLita");
            CardSupply Lita = _cardsProvider.GetCard<Card01117>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Lita, _investigatorsProvider.Second.AidZone).Execute();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE3.Khargath, SceneCORE3.MainPath).Execute().AsCoroutine();


            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Second).Execute();
            yield return ClickedClone(SceneCORE3.Khargath, 2);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Injury.Value, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.Second.Injury.Value, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.Third.Injury.Value, Is.EqualTo(2));
            Assert.That(_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.LitaSacrifice), Is.True);
        }

        [UnityTest]
        public IEnumerator ThrowLitaWithLitaCard()
        {
            yield return BuildCard("01117", _investigatorsProvider.Second);
            SceneCORE3.ExecutePrivateMethod("ThrowLita");
            CardSupply Lita = _cardsProvider.GetCard<Card01117>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Lita, _investigatorsProvider.Second.AidZone).Execute();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE3.Khargath, SceneCORE3.MainPath).Execute().AsCoroutine();


            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Second).Execute();
            yield return ClickedIn(Lita);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Injury.Value, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.Second.Injury.Value, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.Third.Injury.Value, Is.EqualTo(2));
            Assert.That(_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.LitaSacrifice), Is.True);
        }

        [UnityTest]
        public IEnumerator Defeated()
        {
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentPlot, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.PlotCards.ElementAt(2), SceneCORE3.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<RevealGameAction>().SetWith(SceneCORE3.PlotCards.ElementAt(2)).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE3.Khargath, SceneCORE3.MainPath).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(SceneCORE3.Khargath, _investigatorsProvider.First.InvestigatorCard, amountDamage: SceneCORE3.Khargath.Health.Value).Execute().AsCoroutine();

            Assert.That(SceneCORE3.Khargath.Defeated.IsActive, Is.True);
            Assert.That(_investigatorsProvider.First.Xp.Value, Is.GreaterThanOrEqualTo(10));
        }
    }
}
