using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class PrepareGameUseCaseTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _sut;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvide;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PrepareGame()
        {
            _sut.Execute();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.AllInvestigators.Count, Is.EqualTo(2));
            Assert.That(_cardsProvider.GetCard("01160").Info.Code, Is.EqualTo("01160"));
            Assert.That(_chaptersProvide.CurrentScene.Info.Name, Is.EqualTo("Scene1"));
            yield return null;
        }
    }
}
