using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PrepareSceneCORE2Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE2 scene = (SceneCORE2)_chaptersProvider.CurrentScene;
            yield return _preparationSceneCORE2.PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(8));
            Assert.That(scene.Acolits.Where(acolit => acolit.IsInPlay).Count(), Is.EqualTo(3));
            Assert.That(scene.DangerDeckZone.Cards.Count(), Is.EqualTo(20)); //All - Acolics in Play
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
           Is.EqualTo(scene.Fluvial));
        }

        [UnityTest]
        public IEnumerator PrepareScene2Test()
        {
            SceneCORE2 scene = (SceneCORE2)_chaptersProvider.CurrentScene;
            yield return _preparationSceneCORE2.PlayThisInvestigator(_investigatorsProvider.First, withAvatar: false);
            yield return _preparationSceneCORE2.PlayThisInvestigator(_investigatorsProvider.Second, withAvatar: false);
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.PriestGhoulLive, true));
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.HouseUp, true));

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(9));
            Assert.That(scene.Acolits.Where(acolit => acolit.IsInPlay).Count(), Is.EqualTo(1));
            Assert.That(scene.DangerDeckZone.Cards.Count(), Is.EqualTo(23)); //All + GhoulPriest - Acolics in Play
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
           Is.EqualTo(scene.Home));
        }
    }
}
