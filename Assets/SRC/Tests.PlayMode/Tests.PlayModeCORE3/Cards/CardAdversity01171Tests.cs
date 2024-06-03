using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardAdversity01171Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator PlaceEldritchInNearesCultist()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01171 cardAdversity = _cardsProvider.GetCard<Card01171>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.Drew, SceneCORE3.Forest1.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity)).AsCoroutine();

            Assert.That(SceneCORE3.Drew.Eldritch.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator DrawCultist()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01171 cardAdversity = _cardsProvider.GetCard<Card01171>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity)).AsCoroutine();

            Assert.That(_cardsProvider.GetCardsInPlay().OfType<CardCreature>().Any(card => card.HasThisTag(Tag.Cultist)), Is.True);
        }
    }
}