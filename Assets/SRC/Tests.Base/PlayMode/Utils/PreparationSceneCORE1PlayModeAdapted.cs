using MythosAndHorrors.EditMode.Tests;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PreparationSceneCORE1PlayModeAdapted : PreparationScenePlayModeAdapted
    {
        [Inject] private readonly PreparationSceneCORE1 _preparationSceneCORE1;

        /*******************************************************************/
        public SceneCORE1 SceneCORE1 => _preparationSceneCORE1.SceneCORE1;

        protected override MythosAndHorrors.EditMode.Tests.Preparation Preparation => _preparationSceneCORE1;
    }
}
