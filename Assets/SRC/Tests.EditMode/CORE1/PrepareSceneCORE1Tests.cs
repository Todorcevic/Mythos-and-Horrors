using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Linq;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class PrepareSceneCORE1Tests : TestCORE1Base
    {
        [Test]
        public void PrepareSceneCORE1()
        {
            _preparationSceneCORE1.PlayAllInvestigators(withAvatar: false).Wait();

            _gameActionsProvider.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene)).Wait();

            Assert.That(SceneCORE1.GoalZone.Cards.Unique(), Is.EqualTo(SceneCORE1.FirstGoal));
            Assert.That(SceneCORE1.PlotZone.Cards.Unique(), Is.EqualTo(SceneCORE1.FirstPlot));
            Assert.That(SceneCORE1.DangerDeckZone.Cards.Count(), Is.EqualTo(SceneCORE1.StartDangerCards.Count()));
        }
    }
}
