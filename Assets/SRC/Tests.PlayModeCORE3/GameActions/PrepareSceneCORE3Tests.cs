using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PrepareSceneCORE3Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE3 scene = (SceneCORE3)_chaptersProvider.CurrentScene;
            yield return _preparationScene.PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(5));
            Assert.That(scene.DangerDeckZone.Cards.Where(card => scene.AllAgents.Contains(card)).Count() < scene.AllAgents.Count(), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
           Is.EqualTo(scene.MainPath));
        }
    }
}
