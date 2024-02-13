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
    public class AvatarTest : TestBase
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly InvestigatorLoaderUseCase _investigatorLoaderUseCase;
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly FilesPath _filesPath;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Load_Avatar()
        {
            _investigatorLoaderUseCase.Execute(_filesPath.JSON_INVESTIGATOR_PATH("01501"));
            _investigatorLoaderUseCase.Execute(_filesPath.JSON_INVESTIGATOR_PATH("01502"));
            _investigatorLoaderUseCase.Execute(_filesPath.JSON_INVESTIGATOR_PATH("01503"));
            _investigatorLoaderUseCase.Execute(_filesPath.JSON_INVESTIGATOR_PATH("01504"));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_avatarViewsManager.AllAvatars.Count, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator Show_Turns()
        {
            Investigator doc = new() { InvestigatorCard = _cardBuilder.BuildOfType<CardInvestigator>() };
            AvatarView avatarView = _avatarViewsManager.GetVoid();
            avatarView.Init(doc);

            avatarView.ShowTurns(3);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(avatarView.GetPrivateMember<TurnController>("_turnController").ActiveTurnsCount, Is.EqualTo(3));
        }
    }
}