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
        public IEnumerator Load_Avatar()
        {
            DEBUG_MODE = true;
            _adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH("01501"));
            _adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH("01502"));
            _adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH("01503"));
            _adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH("01504"));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_avatarViewsManager.AllAvatars.Count, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator Show_Turns()
        {
            //DEBUG_MODE = true;
            Adventurer doc = new() { AdventurerCard = _cardBuilder.BuildOfType<CardAdventurer>() };
            AvatarView avatarView = _avatarViewsManager.GetVoid();
            avatarView.Init(doc);

            avatarView.ShowTurns(3);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(avatarView.GetComponentsInChildren<TurnView>().Count(), Is.EqualTo(3));
        }
    }
}