using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class PrepareSceneCORE3Tests : TestCORE3Base
    {

        [Test]
        public async void PrepareSceneCORE3()
        {
            await _preparationSceneCORE3.PlayAllInvestigators(withAvatar: false);

            await _gameActionsProvider.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            Assert.That(SceneCORE3.GoalZone.Cards.Unique(), Is.EqualTo(SceneCORE3.FirstGoal));
            Assert.That(SceneCORE3.PlotZone.Cards.Unique(), Is.EqualTo(SceneCORE3.FirstPlot));
            Assert.That(SceneCORE3.DangerDeckZone.Cards.Count(), Is.EqualTo(SceneCORE3.StartDangerCards.Count()));
        }

    }
}
