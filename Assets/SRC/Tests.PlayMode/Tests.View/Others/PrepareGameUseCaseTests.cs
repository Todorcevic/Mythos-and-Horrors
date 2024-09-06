using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayModeCORE1.Tests;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class PrepareGameUseCaseTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PrepareGame()
        {
            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.AllInvestigators.Count(), Is.EqualTo(4));
            Assert.That(_cardsProvider.GetCard<Card01160>().Info.Code, Is.EqualTo("01160"));
            yield return null;
        }
    }
}
