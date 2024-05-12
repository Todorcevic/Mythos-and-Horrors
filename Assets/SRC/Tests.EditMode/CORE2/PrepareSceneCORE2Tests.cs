using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Linq;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class PrepareSceneCORE2Tests : TestCORE2Base
    {
        [Test]
        public async void PrepareSceneCORE2()
        {
            await _preparationSceneCORE2.PlayAllInvestigators(withAvatar: false);

            await _gameActionsProvider.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            Assert.That(SceneCORE2.GoalZone.Cards.Unique(), Is.EqualTo(SceneCORE2.FirstGoal));
            Assert.That(SceneCORE2.PlotZone.Cards.Unique(), Is.EqualTo(SceneCORE2.FirstPlot));
            Assert.That(SceneCORE2.DangerDeckZone.Cards.Count(), Is.EqualTo(SceneCORE2.StartDangerCards.Count() - 2));  // -2 because acolitcs for 4 Investigators InPlay
        }
    }
}
