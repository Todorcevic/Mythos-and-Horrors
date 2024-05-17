using MythosAndHorrors.EditMode.Tests;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PreparationSceneCORE2PlayModeAdapted : PreparationScenePlayModeAdapted
    {
        [Inject] private readonly PreparationSceneCORE2 _preparationSceneCORE2;

        /*******************************************************************/
        public SceneCORE2 SceneCORE2 => _preparationSceneCORE2.SceneCORE2;

        protected override MythosAndHorrors.EditMode.Tests.Preparation Preparation => _preparationSceneCORE2;
    }
}
