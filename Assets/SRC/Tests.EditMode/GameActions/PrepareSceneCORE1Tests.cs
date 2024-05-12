using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public class PrepareSceneCORE1Tests : SetupAutoInject
    {
        [Inject] private readonly PrepareGameRulesUseCase _prepareGameRulesUseCase;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly PreparationSceneCORE1 _preparationSceneCORE1;

        SceneCORE1 Scene => (SceneCORE1)_chaptersProvider.CurrentScene;

        /*******************************************************************/
        [Test]
        public async void PrepareSceneCORE1()
        {
            _prepareGameRulesUseCase.Execute();
            await _preparationSceneCORE1.PlayAllInvestigators(withAvatar: false);
            await _gameActionsProvider.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            Assert.That(Scene.GoalZone.Cards.Unique(), Is.EqualTo(Scene.FirstGoal));
            Assert.That(Scene.PlotZone.Cards.Unique(), Is.EqualTo(Scene.FirstPlot));
            Assert.That(Scene.DangerDeckZone.Cards.Count(), Is.EqualTo(Scene.RealDangerCards.Count()));

        }
    }
}
