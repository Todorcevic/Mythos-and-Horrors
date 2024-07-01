using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardPlace01114Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator TakeDamageInvestigatorWhenEnter()
        {
            CardPlace Cellar = _cardsProvider.GetCard<Card01114>();
            Investigator investigator = _investigatorsProvider.AllInvestigators.First();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Cellar, _chaptersProvider.CurrentScene.GetPlaceZone(1, 3)).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, Cellar).Start().AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
        }
    }
}
