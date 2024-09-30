using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using UnityEngine.UI;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class ShowHistoryests : PlayModeTestsBase
    {
        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Show_History()
        {
            History sutHistory = new()
            {
                Title = "Title",
                Description = "Description",
                Image = "CORE2_RESOLUTION2",
            };

            do
            {
                yield return _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(sutHistory).Execute().AsCoroutine();
            }
            while (DEBUG_MODE);

            Assert.That(_showHistoryComponent.GetPrivateMember<TextMeshProUGUI>("_title").text, Is.EqualTo(sutHistory.Title));
            Assert.That(_showHistoryComponent.GetPrivateMember<TextMeshProUGUI>("_content").text, Is.EqualTo(sutHistory.Description));
            Assert.That(_showHistoryComponent.GetPrivateMember<Image>("_screen").sprite.name, Is.EqualTo(sutHistory.Image));
        }
    }
}
