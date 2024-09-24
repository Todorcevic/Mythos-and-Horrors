using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardPlace01113Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator TakeFearOneInvestigatorWhenEnter()
        {
            CardPlace Attick = _cardsProvider.GetCard<Card01113>();
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Attick, _chaptersProvider.CurrentScene.GetPlaceZone(1, 3)).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, Attick).Execute().AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator TakeFearAllInvestigatorWhenEnter()
        {
            CardPlace Attick = _cardsProvider.GetCard<Card01113>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Attick, _chaptersProvider.CurrentScene.GetPlaceZone(1, 3)).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay, Attick).Execute().AsCoroutine();

            Assert.That(_investigatorsProvider.First.FearRecived.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Second.FearRecived.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Third.FearRecived.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Fourth.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
