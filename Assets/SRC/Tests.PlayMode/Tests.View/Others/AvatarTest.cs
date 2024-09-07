using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class AvatarTest : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Load_Avatar()
        {
            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            //yield return new WaitForSeconds(3);
            Assert.That(_avatarViewsManager.AllAvatars.Count, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator Show_Turns()
        {
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(_investigatorsProvider.First.CurrentActions, 3).Execute().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.First).GetPrivateMember<ActionController>("_actionController").ActiveTurnsCount, Is.EqualTo(3));
        }
    }
}