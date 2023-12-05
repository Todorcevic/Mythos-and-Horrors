using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class AvatarTest : TestBase
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly AdventurerLoaderUseCase _adventurerLoaderUseCase;
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly FilesPath _filesPath;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Initialize_Avatar()
        {
            Adventurer doc = new() { AdventurerCard = _cardBuilder.BuildOfType<CardAdventurer>() };
            _avatarViewsManager.Init(doc);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_avatarViewsManager.Get(doc).Adventurer, Is.EqualTo(doc));
        }

        [UnityTest]
        public IEnumerator Load_Avatar()
        {
            _adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH("01501"));
            _adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH("01502"));
            //_adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH("01503"));
            //_adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH("01504"));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_avatarViewsManager.AllAvatars.Count, Is.EqualTo(2));
        }
    }
}