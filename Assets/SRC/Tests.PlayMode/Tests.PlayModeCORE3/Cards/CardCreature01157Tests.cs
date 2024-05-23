using MythosAndHorrors.GameRules;
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
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE3.Urmodoth, SceneCORE3.MainPath)).AsCoroutine();
            yield return _gameActionsProvider.Create(new CreatureConfrontAttackGameAction()).AsCoroutine();

            Assert.That(_investigatorsProvider.First.DamageRecived, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.DamageRecived, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Third.DamageRecived, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.First.FearRecived, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.FearRecived, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Third.FearRecived, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator OpportunityAttack()
        {
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE3.Urmodoth, SceneCORE3.MainPath)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Second));
            yield return ClickedIn(SceneCORE3.Forests.First());
            yield return ClickedIn(_investigatorsProvider.Second.InvestigatorCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.DamageRecived, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.DamageRecived, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Third.DamageRecived, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.First.FearRecived, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.FearRecived, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Third.FearRecived, Is.EqualTo(0));
            Assert.That(SceneCORE3.Urmodoth.CurrentPlace, Is.EqualTo(SceneCORE3.MainPath));
        }

        [UnityTest]
        public IEnumerator ThrowLita()
        {
            yield return BuilCard("01117", _investigatorsProvider.Second);
            CardSupply Lita = _cardsProvider.GetCard<Card01117>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Lita, _investigatorsProvider.Second.AidZone));
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE3.Urmodoth, SceneCORE3.MainPath)).AsCoroutine();


            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Second));
            yield return ClickedClone(SceneCORE3.Urmodoth, 2);
            yield return ClickedIn(_investigatorsProvider.Second.InvestigatorCard);
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
            yield return BuilCard("01117", _investigatorsProvider.Second);
            CardSupply Lita = _cardsProvider.GetCard<Card01117>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Lita, _investigatorsProvider.Second.AidZone));
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE3.Urmodoth, SceneCORE3.MainPath)).AsCoroutine();


            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Second));
            yield return ClickedIn(Lita);
            yield return ClickedIn(_investigatorsProvider.Second.InvestigatorCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Injury.Value, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.Second.Injury.Value, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.Third.Injury.Value, Is.EqualTo(2));
            Assert.That(_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.LitaSacrifice), Is.True);
        }
    }
}
