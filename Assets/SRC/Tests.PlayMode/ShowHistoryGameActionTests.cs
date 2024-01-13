using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ShowHistoryGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Show_History()
        {
            _prepareGameUse.Execute();
            History sutHistory = new()
            {
                Title = "Title",
                Description = "Description",
                Image = "01105",
            };

            WaitToClick().AsTask();

            do
            {
                yield return _gameActionFactory.Create<ShowHistoryGameAction>().Run(sutHistory).AsCoroutine();
            }
            while (DEBUG_MODE);

            Assert.That(_showHistoryComponent.GetPrivateMember<TextMeshProUGUI>("_title").text, Is.EqualTo(sutHistory.Title));
            Assert.That(_showHistoryComponent.GetPrivateMember<TextMeshProUGUI>("_content").text, Is.EqualTo(sutHistory.Description));
            Assert.That(_showHistoryComponent.GetPrivateMember<Image>("_screen").sprite.name, Is.EqualTo(sutHistory.Image));
        }

        private IEnumerator WaitToClick()
        {
            yield return new WaitUntil(() => _showHistoryComponent.GetPrivateMember<Button>("_button").interactable);
            _showHistoryComponent.GetPrivateMember<Button>("_button").onClick.Invoke();
        }
    }
}
