using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ShowHistoryests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Show_History()
        {
            History sutHistory = new()
            {
                Title = "Title",
                Description = "Description",
                Image = "01105",
            };

            do
            {
                yield return _gameActionsProvider.Create(new ShowHistoryGameAction(sutHistory)).AsCoroutine();
            }
            while (DEBUG_MODE);

            Assert.That(_showHistoryComponent.GetPrivateMember<TextMeshProUGUI>("_title").text, Is.EqualTo(sutHistory.Title));
            Assert.That(_showHistoryComponent.GetPrivateMember<TextMeshProUGUI>("_content").text, Is.EqualTo(sutHistory.Description));
            Assert.That(_showHistoryComponent.GetPrivateMember<Image>("_screen").sprite.name, Is.EqualTo(sutHistory.Image));
        }
    }
}
